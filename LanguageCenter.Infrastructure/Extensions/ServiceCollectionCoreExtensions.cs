using LanguageCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionCoreExtensions
    {
        public static IServiceCollection AddDbContexts(
            this IServiceCollection service,
            IConfiguration config)
        {
            var sqlConnectionString = config["ConnectionStrings:SqlConnection"];

            service.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(sqlConnectionString));
            service.AddDatabaseDeveloperPageExceptionFilter();

            return service;
        }
    }
}
