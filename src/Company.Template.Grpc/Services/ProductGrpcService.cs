using Company.Template.Application.Commands;
using Company.Template.Application.Queries;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Company.Template.Grpc.Services;

/// <summary>
/// gRPC service implementation for Product operations
/// </summary>
public class ProductGrpcService : ProductService.ProductServiceBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProductGrpcService> _logger;

    public ProductGrpcService(IMediator mediator, ILogger<ProductGrpcService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public override async Task<ProductResponse> CreateProduct(CreateProductRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC CreateProduct called for product: {Name}", request.Name);

        try
        {
            var command = new CreateProductCommand(request.Name, request.Description, (decimal)request.Price);
            var result = await _mediator.Send(command, context.CancellationToken);

            return new ProductResponse
            {
                Id = result.Id.ToString(),
                Name = result.Name,
                Description = result.Description,
                Price = (double)result.Price,
                CreatedAt = result.CreatedAt.ToString("O"),
                UpdatedAt = result.UpdatedAt?.ToString("O") ?? string.Empty
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product via gRPC");
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }

    public override async Task<ProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC GetProduct called for ID: {Id}", request.Id);

        try
        {
            if (!Guid.TryParse(request.Id, out var productId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid product ID format"));
            }

            var query = new GetProductByIdQuery(productId);
            var result = await _mediator.Send(query, context.CancellationToken);

            if (result == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
            }

            return new ProductResponse
            {
                Id = result.Id.ToString(),
                Name = result.Name,
                Description = result.Description,
                Price = (double)result.Price,
                CreatedAt = result.CreatedAt.ToString("O"),
                UpdatedAt = result.UpdatedAt?.ToString("O") ?? string.Empty
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting product via gRPC");
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }

    public override async Task<ProductResponse> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC UpdateProduct called for ID: {Id}", request.Id);

        try
        {
            if (!Guid.TryParse(request.Id, out var productId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid product ID format"));
            }

            var command = new UpdateProductCommand(productId, request.Name, request.Description, (decimal)request.Price);
            var result = await _mediator.Send(command, context.CancellationToken);

            return new ProductResponse
            {
                Id = result.Id.ToString(),
                Name = result.Name,
                Description = result.Description,
                Price = (double)result.Price,
                CreatedAt = result.CreatedAt.ToString("O"),
                UpdatedAt = result.UpdatedAt?.ToString("O") ?? string.Empty
            };
        }
        catch (InvalidOperationException ex)
        {
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
        catch (RpcException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product via gRPC");
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }

    public override async Task<DeleteProductResponse> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC DeleteProduct called for ID: {Id}", request.Id);

        try
        {
            if (!Guid.TryParse(request.Id, out var productId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid product ID format"));
            }

            var command = new DeleteProductCommand(productId);
            var result = await _mediator.Send(command, context.CancellationToken);

            return new DeleteProductResponse
            {
                Success = result,
                Message = result ? "Product deleted successfully" : "Product not found"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product via gRPC");
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }

    public override async Task<ListProductsResponse> ListProducts(ListProductsRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC ListProducts called with page {PageNumber}, size {PageSize}", 
            request.PageNumber, request.PageSize);

        try
        {
            var query = new ListProductsQuery(request.PageNumber, request.PageSize);
            var result = await _mediator.Send(query, context.CancellationToken);

            var response = new ListProductsResponse
            {
                TotalCount = result.TotalCount,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                HasNextPage = result.HasNextPage,
                HasPreviousPage = result.HasPreviousPage
            };

            foreach (var product in result.Products)
            {
                response.Products.Add(new ProductResponse
                {
                    Id = product.Id.ToString(),
                    Name = product.Name,
                    Description = product.Description,
                    Price = (double)product.Price,
                    CreatedAt = product.CreatedAt.ToString("O"),
                    UpdatedAt = product.UpdatedAt?.ToString("O") ?? string.Empty
                });
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing products via gRPC");
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }
}