namespace Company.Template.Infrastructure.Messaging.IntegrationEvents;

/// <summary>
/// Integration event published when a product is updated
/// </summary>
public record ProductUpdatedIntegrationEvent(
    Guid ProductId,
    string Name,
    string Description,
    decimal Price,
    DateTime UpdatedAt,
    string EventId = default!,
    DateTime EventTime = default!)
{
    public ProductUpdatedIntegrationEvent(
        Guid productId,
        string name,
        string description,
        decimal price,
        DateTime updatedAt) : this(productId, name, description, price, updatedAt, Guid.NewGuid().ToString(), DateTime.UtcNow)
    {
    }
}