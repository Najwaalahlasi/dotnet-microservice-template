using Company.Template.Shared.DTOs;
using MediatR;

namespace Company.Template.Application.Queries;

/// <summary>
/// Query to get a product by its identifier
/// </summary>
public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto?>;