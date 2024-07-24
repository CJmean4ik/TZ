using Application.Requests;
using Application.Shared;
using Domain.Contracts;
using Domain.Generals;
using Domain.Models;
using Domain.Repositories;
using Infrastructure.Cache;
using Infrastructure.Cash.Models;
using Microsoft.AspNetCore.SignalR;

namespace Application.Services;

public class ChatHubService
{
    private readonly ICaching _cache;
    private readonly IChatRepository _chatRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;

    public ChatHubService(ICaching cache, 
        IChatRepository chatRepository,
        IMessageRepository messageRepository, IUserRepository userRepository)
    {
        _cache = cache;
        _chatRepository = chatRepository;
        _messageRepository = messageRepository;
        _userRepository = userRepository;
    }
    
    public async Task<Result> CreateAsync(ChatHubRequest request, HubServiceParameters hubParameters)
    {
        var user = await _userRepository.GetUserByNameAsync(request.UserName);
        if (user.IsFailure)
            return Result.Failure<string>(user.Error);
        
        var chat = Chat.Create(Guid.NewGuid(), request.ChatRoomName, user.Value!);
        if (user.IsFailure)
            return Result.Failure<string>(chat.Error);

        var result = await _chatRepository.CreateAsync(chat.Value!);
        if (result.IsFailure)
            return Result.Failure<string>(result.Error);

        await hubParameters.Group.AddToGroupAsync(hubParameters.HubContext.ConnectionId, request.ChatRoomName);
        var hubCashedContext = new HubChatDataCashing()
        {
            UserId = user.Value.Id,
            ChatId = chat.Value.Id,
            ChatRoomName = request.ChatRoomName,
            UserName = user.Value.Name
        };
        var cashResult = await _cache.AddToCashAsync(hubParameters.HubContext.ConnectionId, hubCashedContext);
        return Result.Success("Hub successfully created");
    }
    public async Task<Result> JoinAsync(ChatHubRequest? context, HubServiceParameters hubParameters)
    {
        ResultT<HubChatDataCashing?> cashedContext = await 
            _cache.ReadFromCashAsync<HubChatDataCashing,string>(hubParameters.HubContext.ConnectionId)!;
        
        if (cashedContext!.Value is null)
        {
            var result = await _cache.AddToCashAsync(hubParameters.HubContext.ConnectionId,context);
            if (result.IsFailure)
                return Result.Failure<string>(result.Error);
        }

        var chat = await _chatRepository.GetChatByNameAsync(context.ChatRoomName);
        if (chat.IsFailure)
            return Result.Failure(new Error(ErrorCodes.NotFounded,
                $"Chat room by name: {context.ChatRoomName} not founded"));

        await hubParameters.Group.AddToGroupAsync(hubParameters.HubContext.ConnectionId, context.ChatRoomName);
        await hubParameters.Clients.Groups(context!.ChatRoomName)
            .ReceiveMessage("Chat notificator", $"{context.UserName} joined to chat");
        return Result.Success("The connection was successful");
    }
    public async Task<Result> SendMessageAsync(string messageForSend, HubServiceParameters hubParameters)
    {
        ResultT<HubChatDataCashing?> cashedContext = await 
            _cache.ReadFromCashAsync<HubChatDataCashing,string>(hubParameters.HubContext.ConnectionId)!;
        
        if (cashedContext!.Value is null)
            return Result.Failure(new Error(ErrorCodes.ValueNull,"It is not possible to send a message because you are not connected to the chat"));


        var chatRepositoryResult = await _chatRepository.GetChatByNameAsync(cashedContext.Value.ChatRoomName);
        var userResult = User.Create(cashedContext.Value.UserId, cashedContext.Value.UserName);
        var messageResult = Message.Create(Guid.NewGuid(), messageForSend, userResult.Value!, chatRepositoryResult.Value!);

        var result = await _messageRepository.CreateAsync(messageResult.Value!);

        if (result.IsFailure)
            return Result.Failure(result.Error);
        
        await hubParameters.Clients.Groups(cashedContext.Value.ChatRoomName)
            .ReceiveMessage(cashedContext.Value.UserName, messageForSend);
        
        return Result.Success("The message sent");
    }
    public async Task<Result> DeleteAsync(ChatHubRequest request, HubServiceParameters hubParameters)
    {
        ResultT<HubChatDataCashing?> cashedContext = await 
            _cache.ReadFromCashAsync<HubChatDataCashing,string>(hubParameters.HubContext.ConnectionId)!;
        
        var chatRepositoryResult = await _chatRepository.GetChatByNameAsync(request.ChatRoomName)!;
        
        if (chatRepositoryResult.IsFailure || string.IsNullOrWhiteSpace(cashedContext.Value?.UserId.ToString()))
            return Result.Failure(new Error(chatRepositoryResult.Error.Code,"Cannot delete chat"));

        if (chatRepositoryResult.Value?.UserId != cashedContext.Value.UserId)
            return Result.Failure(new Error(ErrorCodes.DenialAccess,"Only the creator can delete the chat"));
        
        Chat chat = chatRepositoryResult.Value!;
        
        var deleteResult = await _chatRepository.DeleteAsync(chat);
        if (deleteResult.IsFailure)
            return Result.Failure(deleteResult.Error);
        
        await hubParameters.Group.RemoveFromGroupAsync(hubParameters.HubContext.ConnectionId,chat.Name);
        return Result.Success("Chat room has been successfully removed");
    }
}