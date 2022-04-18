using LanguageCenter.Core.Services;
using LanguageCenter.Core.Services.Contracts;
using LanguageCenter.Infrastructure.Data;
using LanguageCenter.Infrastructure.Data.Models;
using LanguageCenter.Infrastructure.Data.Repository.ApplicationRepository;
using LanguageCenter.Infrastructure.Data.Repository.Contracts;
using LanguageCenter.Infrastructure.Services;
using LanguageCenter.Infrastructure.Services.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {

        public static IServiceCollection AddServices(
            this IServiceCollection service)
        {
            service
                .AddScoped<HttpClient>()
                .AddScoped<IApplicationRepository, ApplicationRepository>()
                .AddScoped<ICourseService, CourseService>()
                .AddScoped<ILanguageService, LanguageService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<ITeacherService, TeacherService>();

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

        public static IServiceCollection AddExternalAuthentications(
            this IServiceCollection service,
            IConfiguration config)
        {
            service.AddAuthentication()
                .AddGoogle(opt =>
                {
                    IConfigurationSection googleSection =
                        config.GetSection("Google");

                    opt.ClientId = googleSection["ClientId"];
                    opt.ClientSecret = googleSection["clientSecret"];
                });


            return service;
        }
    }
}
