using MediatR;

namespace Company.Template.Application.Commands;

/// <summary>
/// Command to delete a product
/// </summary>
public record DeleteProductCommand(Guid Id) : IRequest<bool>;