using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ImageManagement.Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }

        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            return services.AddApplicationServices();
        }
    }
}
