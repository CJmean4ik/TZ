using Api.Contracts;
using Domain.Repositories;

namespace Api.Shared;

public class ChatServiceParameters
{
    public IChatRepository ChatRepository { get; set; }
    public IResponceFactory ResponceFactory { get; set; }
    public HttpContext Context { get; set; }

    public ChatServiceParameters(IServiceProvider serviceProvider)
    {
        ChatRepository = serviceProvider.GetRequiredService<IChatRepository>();
        ResponceFactory = serviceProvider.GetRequiredService<IResponceFactory>();
        var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
        Context = httpContextAccessor.HttpContext;
    }
}