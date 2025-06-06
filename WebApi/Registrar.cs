using Infrastructure.EntityFramework;
using Infrastructure.Repositories.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Services.Abstractions;
using Services.Implementations;
using Services.Repositories.Abstractions;
using WebApi.HealthChecks;
using WebApi.Settings;

namespace WebApi
{
    /// <summary>
    /// Регистратор сервиса.
    /// </summary>
    public static class Registrar
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            var applicationSettings = configuration.Get<ApplicationSettings>();
            services.AddSingleton(applicationSettings)
                    .AddSingleton((IConfigurationRoot)configuration)
                    .InstallServices()
                    .ConfigureContext(applicationSettings.ConnectionString)
                    .InstallRepositories();
            
            services.AddHealthChecks()
                .AddCheck<LivenessHealthCheck>(
                    "SampleHealthCheck",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new[]
                    {
                        "SampleHealthCheck"
                    })
                .AddCheck<FailedHealthCheck>(
                    "failed_check",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new[]
                    {
                        "failed_check"
                    });
            return services;
        }
        
        private static IServiceCollection InstallServices(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient<ICourseService, CourseService>()
                .AddTransient<ILessonService, LessonService>();
            return serviceCollection;
        }
        
        private static IServiceCollection InstallRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient<ICourseRepository, CourseRepository>()
                .AddTransient<ILessonRepository, LessonRepository>()
                .AddTransient<IUnitOfWork, UnitOfWork>();
            return serviceCollection;
        }
    }
}