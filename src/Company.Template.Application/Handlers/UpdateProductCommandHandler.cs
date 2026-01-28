using AutoMapper;
using Company.Template.Application.Commands;
using Company.Template.Domain.Events;
using Company.Template.Domain.Repositories;
using Company.Template.Shared.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Company.Template.Application.Handlers;

/// <summary>
/// Handler for UpdateProductCommand
/// </summary>
public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<UpdateProductCommandHandler> _logger;

    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        IMapper mapper,
        IMediator mediator,
        ILogger<UpdateProductCommandHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating product with ID: {ProductId}", request.Id);

        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null)
        {
            _logger.LogWarning("Product not found with ID: {ProductId}", request.Id);
            throw new InvalidOperationException($"Product with ID {request.Id} not found");
        }

        product.Update(request.Name, request.Description, request.Price);
        
        var updatedProduct = await _productRepository.UpdateAsync(product, cancellationToken);
        
        // Publish domain event
        var domainEvent = new ProductUpdatedEvent(
            updatedProduct.Id,
            updatedProduct.Name,
            updatedProduct.Description,
            updatedProduct.Price,
            updatedProduct.UpdatedAt ?? DateTime.UtcNow);
            
        await _mediator.Publish(domainEvent, cancellationToken);

        _logger.LogInformation("Product updated successfully with ID: {ProductId}", updatedProduct.Id);

        return _mapper.Map<ProductDto>(updatedProduct);
    }
}