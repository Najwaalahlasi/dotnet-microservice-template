using Company.Template.Shared.DTOs;
using MediatR;

namespace Company.Template.Application.Commands;

/// <summary>
/// Command to update an existing product
/// </summary>
public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    decimal Price) : IRequest<ProductDto>;