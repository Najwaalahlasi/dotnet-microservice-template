using MediatR;

namespace Company.Template.Domain.Events;

/// <summary>
/// Domain event raised when a product is deleted
/// </summary>
public record ProductDeletedEvent(
    Guid ProductId,
    DateTime DeletedAt) : INotification;