using Company.Template.Domain.Repositories;
using Company.Template.Infrastructure.Data;
using Company.Template.Infrastructure.Messaging;
using Company.Template.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Reflection;

namespace Company.Template.Infrastructure;

/// <summary>
/// Dependency injection configuration for Infrastructure layer
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds infrastructure services to the service collection
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Entity Framework (with fallback to in-memory for demo)
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (!string.IsNullOrEmpty(connectionString) && !connectionString.Contains("localhost"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
        }
        else
        {
            // Use in-memory database for demo/development when PostgreSQL is not available
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("DemoDb");
            });
        }

        // Add repositories
        services.AddScoped<IProductRepository, ProductRepository>();

        // Add RabbitMQ (with fallback for demo)
        try
        {
            var rabbitConnectionString = configuration.GetConnectionString("RabbitMQ");
            if (!string.IsNullOrEmpty(rabbitConnectionString) && !rabbitConnectionString.Contains("localhost"))
            {
                services.AddSingleton<IConnection>(serviceProvider =>
                {
                    var logger = serviceProvider.GetRequiredService<ILogger<ConnectionFactory>>();
                    var factory = new ConnectionFactory();
                    factory.Uri = new Uri(rabbitConnectionString);
                    return factory.CreateConnection();
                });
                services.AddScoped<IMessagePublisher, RabbitMqPublisher>();
            }
            else
            {
                // Use a no-op message publisher for demo
                services.AddScoped<IMessagePublisher, NoOpMessagePublisher>();
            }
        }
        catch (Exception)
        {
            // Use a no-op message publisher if RabbitMQ setup fails
            services.AddScoped<IMessagePublisher, NoOpMessagePublisher>();
        }

        // Add MediatR handlers from this assembly
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}