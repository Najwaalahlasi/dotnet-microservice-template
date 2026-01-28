using AutoMapper;
using Company.Template.Application.Queries;
using Company.Template.Domain.Repositories;
using Company.Template.Shared.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Company.Template.Application.Handlers;

/// <summary>
/// Handler for GetProductByIdQuery
/// </summary>
public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductByIdQueryHandler> _logger;

    public GetProductByIdQueryHandler(
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<GetProductByIdQueryHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting product with ID: {ProductId}", request.Id);

        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (product == null)
        {
            _logger.LogInformation("Product not found with ID: {ProductId}", request.Id);
            return null;
        }

        _logger.LogInformation("Product found with ID: {ProductId}", request.Id);
        return _mapper.Map<ProductDto>(product);
    }
}