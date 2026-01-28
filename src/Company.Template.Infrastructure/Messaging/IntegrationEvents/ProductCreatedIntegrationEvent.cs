namespace Company.Template.Infrastructure.Messaging.IntegrationEvents;

/// <summary>
/// Integration event published when a product is created
/// </summary>
public record ProductCreatedIntegrationEvent(
    Guid ProductId,
    string Name,
    string Description,
    decimal Price,
    DateTime CreatedAt,
    string EventId = default!,
    DateTime EventTime = default!)
{
    public ProductCreatedIntegrationEvent(
        Guid productId,
        string name,
        string description,
        decimal price,
        DateTime createdAt) : this(productId, name, description, price, createdAt, Guid.NewGuid().ToString(), DateTime.UtcNow)
    {
    }
}