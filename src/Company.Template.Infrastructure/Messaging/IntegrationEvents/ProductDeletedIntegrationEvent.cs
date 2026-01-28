namespace Company.Template.Infrastructure.Messaging.IntegrationEvents;

/// <summary>
/// Integration event published when a product is deleted
/// </summary>
public record ProductDeletedIntegrationEvent(
    Guid ProductId,
    DateTime DeletedAt,
    string EventId = default!,
    DateTime EventTime = default!)
{
    public ProductDeletedIntegrationEvent(
        Guid productId,
        DateTime deletedAt) : this(productId, deletedAt, Guid.NewGuid().ToString(), DateTime.UtcNow)
    {
    }
}