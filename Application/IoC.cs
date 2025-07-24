using Application.Handlers;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class IoC
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddScoped<IAccountHandler, AccountHandler>();
        services.AddScoped<IClientHandler, ClientHandler>();
        services.AddScoped<IMovementHandler, MovementHandler>();
    }
}