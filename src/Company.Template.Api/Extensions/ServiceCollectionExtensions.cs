using Microsoft.Extensions.DependencyInjection;

namespace Company.Template.Api.Extensions;

/// <summary>
/// Extension methods for IServiceCollection
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds observability services (placeholder for OpenTelemetry)
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddObservability(this IServiceCollection services, IConfiguration configuration)
    {
        // Placeholder for OpenTelemetry configuration
        // Add OpenTelemetry packages when upgrading to .NET 8+
        return services;
    }

    /// <summary>
    /// Adds resilience patterns using Polly
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddResilience(this IServiceCollection services)
    {
        // Add HTTP client (Polly integration can be added later)
        services.AddHttpClient("resilient-client");
        return services;
    }
}