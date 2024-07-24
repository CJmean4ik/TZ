using Application.Requests;
using Application.Services;
using Domain.Contracts;
using Domain.Generals;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs;

public class ChatHub : Hub<IMessageHandler>
{
    private readonly ChatHubService _chatHubService;

    public ChatHub(ChatHubService chatHubService)
    {
        _chatHubService = chatHubService;
    }

    public async Task<Result> CreateChatAsync(ChatHubRequest request)
    {
        var result = await _chatHubService.CreateAsync(request, Groups, Context);
        return result;
    }

    public async Task<Result> JoinToChatAsync(ChatHubRequest? request)
    {
        var result = await _chatHubService.JoinAsync(request, Clients, Context,Groups);
        return result;
    }

    public async Task<Result> SendMessageAsync(string message)
    {
        var result = await _chatHubService.SendMessageAsync(message, Context, Clients);
        return result;
    }
    
    public async Task<Result> DeleteChatAsync(ChatHubRequest request)
    {
        var result = await _chatHubService.DeleteAsync(request,Groups,Context);
        return result;
    }
}
    