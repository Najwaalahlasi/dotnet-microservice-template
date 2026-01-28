namespace Company.Template.Shared.DTOs;

/// <summary>
/// Data transfer object for Product
/// </summary>
public record ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

/// <summary>
/// Data transfer object for creating a product
/// </summary>
public record CreateProductDto(
    string Name,
    string Description,
    decimal Price);

/// <summary>
/// Data transfer object for updating a product
/// </summary>
public record UpdateProductDto(
    string Name,
    string Description,
    decimal Price);

/// <summary>
/// Paginated response for products
/// </summary>
public record ProductsPagedResponse(
    IEnumerable<ProductDto> Products,
    int TotalCount,
    int PageNumber,
    int PageSize,
    bool HasNextPage,
    bool HasPreviousPage);