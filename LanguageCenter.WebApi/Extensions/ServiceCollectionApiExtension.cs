using LanguageCenter.Core.Services;
using LanguageCenter.Core.Services.Contracts;
using LanguageCenter.Infrastructure.Data.Repository.ApplicationRepository;
using LanguageCenter.Infrastructure.Data.Repository.Contracts;
using LanguageCenter.Infrastructure.Services;
using LanguageCenter.Infrastructure.Services.Contracts;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionApiExtension
    {
        public static IServiceCollection AddServices(
            this IServiceCollection service)
        {
            service
                .AddScoped<IApplicationRepository, ApplicationRepository>()
                .AddScoped<ICourseService, CourseService>()
                .AddScoped<ILanguageService, LanguageService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<ITeacherService, TeacherService>();

            return service;
        }
    }
}
