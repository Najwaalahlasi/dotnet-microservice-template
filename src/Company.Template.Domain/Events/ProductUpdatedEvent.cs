using MediatR;

namespace Company.Template.Domain.Events;

/// <summary>
/// Domain event raised when a product is updated
/// </summary>
public record ProductUpdatedEvent(
    Guid ProductId,
    string Name,
    string Description,
    decimal Price,
    DateTime UpdatedAt) : INotification;