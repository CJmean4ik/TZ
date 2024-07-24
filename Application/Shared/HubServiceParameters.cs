using Domain.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace Application.Shared;

public class HubServiceParameters
{
    public IGroupManager Group { get; init; }
    public HubCallerContext HubContext { get; init; }
    public IHubCallerClients<IMessageHandler> Clients { get; init; }

    public HubServiceParameters(IGroupManager group, HubCallerContext hubContext, IHubCallerClients<IMessageHandler> clients)
    {
        Group = group;
        HubContext = hubContext;
        Clients = clients;
    }
}