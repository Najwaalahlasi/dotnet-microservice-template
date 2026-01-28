using Company.Template.Domain.Events;
using Company.Template.Infrastructure.Messaging;
using Company.Template.Infrastructure.Messaging.IntegrationEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Company.Template.Infrastructure.EventHandlers;

/// <summary>
/// Handler for ProductCreatedEvent that publishes integration event
/// </summary>
public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<ProductCreatedEventHandler> _logger;

    public ProductCreatedEventHandler(
        IMessagePublisher messagePublisher,
        ILogger<ProductCreatedEventHandler> logger)
    {
        _messagePublisher = messagePublisher;
        _logger = logger;
    }

    public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling ProductCreatedEvent for product {ProductId}", notification.ProductId);

        var integrationEvent = new ProductCreatedIntegrationEvent(
            notification.ProductId,
            notification.Name,
            notification.Description,
            notification.Price,
            notification.CreatedAt);

        await _messagePublisher.PublishAsync(
            integrationEvent,
            "product.events",
            "product.created",
            cancellationToken);

        _logger.LogInformation("Published ProductCreatedIntegrationEvent for product {ProductId}", notification.ProductId);
    }
}