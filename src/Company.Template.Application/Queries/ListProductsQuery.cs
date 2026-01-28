using Company.Template.Shared.DTOs;
using MediatR;

namespace Company.Template.Application.Queries;

/// <summary>
/// Query to list products with pagination
/// </summary>
public record ListProductsQuery(
    int PageNumber = 1,
    int PageSize = 10) : IRequest<ProductsPagedResponse>;