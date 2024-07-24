using DataAccess.Repositories;
using Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess
{
    //Dependency Injections
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services)
        {
            services.AddDbContext<ChatContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>(); 
            services.AddScoped<IChatRepository, ChatRepository>();
            return services;
        }
    }
}
