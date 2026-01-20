using KatalogKlientow.Infrastructure;
using KatalogKlientow.Models;
using KatalogKlientow.Repositories;
using KatalogKlientow.Services;
using Microsoft.Extensions.DependencyInjection;

namespace KatalogKlientow.Configuration
{
    public static class DIConfig
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddTransient<ClientCatalogForm>();
            services.AddTransient<DatabaseInitializer>();
            services.AddTransient<DatabaseCleaner>();
            services.AddSingleton<ErrorLogger>();
            services.AddSingleton<IAppConfiguration, AppConfiguration>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IClientService, ClientService>();
            services.AddTransient<IModelValidator, ModelValidator>();

            return services;
        }
    }
}
