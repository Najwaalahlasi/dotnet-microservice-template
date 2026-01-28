using MediatR;

namespace Company.Template.Domain.Events;

/// <summary>
/// Domain event raised when a product is created
/// </summary>
public record ProductCreatedEvent(
    Guid ProductId,
    string Name,
    string Description,
    decimal Price,
    DateTime CreatedAt) : INotification;