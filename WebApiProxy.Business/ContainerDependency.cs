
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApiProxy.Business.Interfaces;
using WebApiProxy.Business.Services;

namespace WebApiProxy.Business
{
    public static class ContainerDependency
    {
        public static IServiceCollection AddDependencyBusinessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IIntegrationService, IntegrationService>();
            
            return services;
        }

    }
}
