
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Proyecto.Model.Interfaces;
using Proyecto.Model.Repository;

namespace Proyecto.Model
{
    public static class ContainerDependency
    {
        public static IServiceCollection AddDependencyDataLayer(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<DemoContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("CnnDemo")));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }
    }
}
