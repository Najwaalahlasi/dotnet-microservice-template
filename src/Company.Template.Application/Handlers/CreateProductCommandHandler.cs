using AutoMapper;
using Company.Template.Application.Commands;
using Company.Template.Domain.Entities;
using Company.Template.Domain.Events;
using Company.Template.Domain.Repositories;
using Company.Template.Shared.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Company.Template.Application.Handlers;

/// <summary>
/// Handler for CreateProductCommand
/// </summary>
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateProductCommandHandler> _logger;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IMapper mapper,
        IMediator mediator,
        ILogger<CreateProductCommandHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating product with name: {Name}", request.Name);

        var product = Product.Create(request.Name, request.Description, request.Price);
        
        var createdProduct = await _productRepository.AddAsync(product, cancellationToken);
        
        // Publish domain event
        var domainEvent = new ProductCreatedEvent(
            createdProduct.Id,
            createdProduct.Name,
            createdProduct.Description,
            createdProduct.Price,
            createdProduct.CreatedAt);
            
        await _mediator.Publish(domainEvent, cancellationToken);

        _logger.LogInformation("Product created successfully with ID: {ProductId}", createdProduct.Id);

        return _mapper.Map<ProductDto>(createdProduct);
    }
}