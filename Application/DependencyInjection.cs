using Application.Services;
using Infrastructure.Cash;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection collection)
    {
        collection.AddScoped<ChatHubService>();
        return collection;
    }
}