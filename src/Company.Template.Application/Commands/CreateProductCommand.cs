using Company.Template.Shared.DTOs;
using MediatR;

namespace Company.Template.Application.Commands;

/// <summary>
/// Command to create a new product
/// </summary>
public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price) : IRequest<ProductDto>;