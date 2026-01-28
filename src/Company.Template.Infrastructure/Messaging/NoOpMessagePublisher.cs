using Microsoft.Extensions.Logging;

namespace Company.Template.Infrastructure.Messaging;

/// <summary>
/// No-operation message publisher for demo/development scenarios
/// </summary>
public class NoOpMessagePublisher : IMessagePublisher
{
    private readonly ILogger<NoOpMessagePublisher> _logger;

    public NoOpMessagePublisher(ILogger<NoOpMessagePublisher> logger)
    {
        _logger = logger;
    }

    public async Task PublishAsync<T>(T message, string exchange, string routingKey, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Demo Mode: Would publish message to exchange {Exchange} with routing key {RoutingKey}. Message: {@Message}", 
            exchange, routingKey, message);
        
        await Task.CompletedTask;
    }
}