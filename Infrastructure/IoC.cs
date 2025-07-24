using ClientDirectory.Domain.Interfaces;
using Infrastructure.Context;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class IoC
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IClientDirectoryDbContext, ClientDirectoryDbContext>();
            services.AddScoped<IRepository, Repository>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ClientDirectoryDbContext>(options => options.UseSqlServer(connectionString));
        }

    }
}