using Company.Template.Domain.Events;
using Company.Template.Infrastructure.Messaging;
using Company.Template.Infrastructure.Messaging.IntegrationEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Company.Template.Infrastructure.EventHandlers;

/// <summary>
/// Handler for ProductDeletedEvent that publishes integration event
/// </summary>
public class ProductDeletedEventHandler : INotificationHandler<ProductDeletedEvent>
{
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<ProductDeletedEventHandler> _logger;

    public ProductDeletedEventHandler(
        IMessagePublisher messagePublisher,
        ILogger<ProductDeletedEventHandler> logger)
    {
        _messagePublisher = messagePublisher;
        _logger = logger;
    }

    public async Task Handle(ProductDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling ProductDeletedEvent for product {ProductId}", notification.ProductId);

        var integrationEvent = new ProductDeletedIntegrationEvent(
            notification.ProductId,
            notification.DeletedAt);

        await _messagePublisher.PublishAsync(
            integrationEvent,
            "product.events",
            "product.deleted",
            cancellationToken);

        _logger.LogInformation("Published ProductDeletedIntegrationEvent for product {ProductId}", notification.ProductId);
    }
}