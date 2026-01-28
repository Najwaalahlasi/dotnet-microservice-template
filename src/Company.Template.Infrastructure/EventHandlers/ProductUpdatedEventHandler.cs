using Company.Template.Domain.Events;
using Company.Template.Infrastructure.Messaging;
using Company.Template.Infrastructure.Messaging.IntegrationEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Company.Template.Infrastructure.EventHandlers;

/// <summary>
/// Handler for ProductUpdatedEvent that publishes integration event
/// </summary>
public class ProductUpdatedEventHandler : INotificationHandler<ProductUpdatedEvent>
{
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<ProductUpdatedEventHandler> _logger;

    public ProductUpdatedEventHandler(
        IMessagePublisher messagePublisher,
        ILogger<ProductUpdatedEventHandler> logger)
    {
        _messagePublisher = messagePublisher;
        _logger = logger;
    }

    public async Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling ProductUpdatedEvent for product {ProductId}", notification.ProductId);

        var integrationEvent = new ProductUpdatedIntegrationEvent(
            notification.ProductId,
            notification.Name,
            notification.Description,
            notification.Price,
            notification.UpdatedAt);

        await _messagePublisher.PublishAsync(
            integrationEvent,
            "product.events",
            "product.updated",
            cancellationToken);

        _logger.LogInformation("Published ProductUpdatedIntegrationEvent for product {ProductId}", notification.ProductId);
    }
}