using Infrastructure.Cache;
using Infrastructure.Cash;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection collection)
    {
        collection.AddMemoryCache();
        collection.AddDistributedMemoryCache();
        collection.AddScoped<ICaching, Caching>();
        return collection;
    }
}