using LanguageCenter.Core.Services;
using LanguageCenter.Core.Services.Contracts;
using LanguageCenter.Infrastructure.Data;
using LanguageCenter.Infrastructure.Data.Models;
using LanguageCenter.Infrastructure.Data.Repository.ApplicationRepository;
using LanguageCenter.Infrastructure.Data.Repository.Contracts;
using LanguageCenter.Infrastructure.Services;
using LanguageCenter.Infrastructure.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {

        public static IServiceCollection AddServices(
            this IServiceCollection service)
        {
            service
                .AddScoped<IApplicationRepository, ApplicationRepository>()
                .AddScoped<ICourseService, CourseService>()
                .AddScoped<ILanguageService, LanguageService>()
                .AddScoped<IUserService, UserService>();

            return service;
        }

        public static IServiceCollection AddDefaultIdentities(
            this IServiceCollection service)
        {
            service.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            return service;
        }

        public static IServiceCollection AddDbContexts(
            this IServiceCollection service,
            IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            service.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            service.AddDatabaseDeveloperPageExceptionFilter();

            return service;
        }
    }
}
