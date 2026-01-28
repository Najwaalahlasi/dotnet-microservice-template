using AutoMapper;
using Company.Template.Application.Queries;
using Company.Template.Domain.Repositories;
using Company.Template.Shared.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Company.Template.Application.Handlers;

/// <summary>
/// Handler for ListProductsQuery
/// </summary>
public class ListProductsQueryHandler : IRequestHandler<ListProductsQuery, ProductsPagedResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ListProductsQueryHandler> _logger;

    public ListProductsQueryHandler(
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<ListProductsQueryHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductsPagedResponse> Handle(ListProductsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting products page {PageNumber} with size {PageSize}", 
            request.PageNumber, request.PageSize);

        var skip = (request.PageNumber - 1) * request.PageSize;
        var take = request.PageSize;

        var products = await _productRepository.GetAllAsync(skip, take, cancellationToken);
        var totalCount = await _productRepository.GetCountAsync(cancellationToken);

        var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

        var hasNextPage = skip + take < totalCount;
        var hasPreviousPage = request.PageNumber > 1;

        _logger.LogInformation("Retrieved {Count} products out of {TotalCount}", 
            productDtos.Count(), totalCount);

        return new ProductsPagedResponse(
            productDtos,
            totalCount,
            request.PageNumber,
            request.PageSize,
            hasNextPage,
            hasPreviousPage);
    }
}