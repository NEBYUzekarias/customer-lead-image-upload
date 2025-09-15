using ImageManagement.Application.Contracts.Persistence;
using ImageManagement.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManagement.Persistence
{
    public static class PersistenceServicesRegistration
    {
        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ImageManagementConnectionString");
            
            // Use in-memory database if connection string is not available
            if (string.IsNullOrEmpty(connectionString) || connectionString.Contains("localhost"))
            {
                services.AddDbContext<BlogAppDbContext>(opt =>
                    opt.UseInMemoryDatabase("ImageManagementDB"));
            }
            else
            {
                services.AddDbContext<BlogAppDbContext>(opt =>
                    opt.UseNpgsql(connectionString));
            }

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProfileImageRepository, ProfileImageRepository>();

            return services;
        }
    }
}
