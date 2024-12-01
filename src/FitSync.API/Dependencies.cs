using System.Reflection;
using FitSync.Application.Services;
using FitSync.Domain.Interfaces;
using FitSync.Infrastructure.Data;
using FitSync.Infrastructure.FileHandling;
using FitSync.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace FitSync.API;

public static class Dependencies
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationServices();
        services.AddInfrastructure(configuration);
        services.AddRepositories();
        services.AddSwagger();

        return services;
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICsvReader, CsvHelperReader>();
        services.AddScoped<IDataImportService, DataImportService>();
        services.AddScoped<IWorkoutService, WorkoutService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IWorkoutPlanService, WorkoutPlanService>();
        services.AddScoped<ICalendarService, CalendarService>();
        services.AddScoped<IUserPreferencesService, UserPreferencesService>();

        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FitSyncDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<DbContext, FitSyncDbContext>();
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IFitSyncUnitOfWork), typeof(FitSyncUnitOfWork));
        services.AddScoped(typeof(IWorkoutRepository), typeof(WorkoutRepository));
        services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
        services.AddScoped(typeof(IWorkoutPlanRepository), typeof(WorkoutPlanRepository));
        services.AddScoped(typeof(IWorkoutPlanWorkoutRepository), typeof(WorkoutPlanWorkoutRepository));
        services.AddScoped(typeof(IUserPreferencesRepository), typeof(UserPreferencesRepository));

        return services;
    }


    public static void MigrateDatabase(IServiceProvider serviceProvider)
    {
       // TODO: add Environment variable to check if we are in production / local
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<FitSyncDbContext>();
            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while migrating the database.", ex);
        }
    }


    private static IServiceCollection AddSwagger(this IServiceCollection services)
     {
         services.AddSwaggerGen(c =>
         {
             var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
             c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

             c.SwaggerDoc("v1", new OpenApiInfo
             {
                 Title = "FitSync Swagger",
                 Contact = new OpenApiContact
                 {
                     Name = "_",
                     Email = "_",
                     Url = new Uri("https://fitsync.io")
                 }
             });
         });

         return services;
     }
}