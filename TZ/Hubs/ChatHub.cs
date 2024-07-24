using Application.Requests;
using Application.Services;
using Application.Shared;
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
        var hubParameters = CreateHubServiceParameters();
        var result = await _chatHubService.CreateAsync(request,hubParameters);
        return result;
    }

    public async Task<Result> JoinToChatAsync(ChatHubRequest? request)
    {
        var hubParameters = CreateHubServiceParameters();
        var result = await _chatHubService.JoinAsync(request, hubParameters);
        return result;
    }

    public async Task<Result> SendMessageAsync(string message)
    {
        var hubParameters = CreateHubServiceParameters();
        var result = await _chatHubService.SendMessageAsync(message, hubParameters);
        return result;
    }
    
    public async Task<Result> DeleteChatAsync(ChatHubRequest request)
    {
        var hubParameters = CreateHubServiceParameters();
        var result = await _chatHubService.DeleteAsync(request, hubParameters);
        return result;
    }

    private HubServiceParameters CreateHubServiceParameters() => new HubServiceParameters(Groups,Context,Clients);
}
    