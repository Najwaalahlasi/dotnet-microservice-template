using Company.Template.Application.Commands;
using Company.Template.Domain.Events;
using Company.Template.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Company.Template.Application.Handlers;

/// <summary>
/// Handler for DeleteProductCommand
/// </summary>
public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<DeleteProductCommandHandler> _logger;

    public DeleteProductCommandHandler(
        IProductRepository productRepository,
        IMediator mediator,
        ILogger<DeleteProductCommandHandler> logger)
    {
        _productRepository = productRepository;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting product with ID: {ProductId}", request.Id);

        var deleted = await _productRepository.DeleteAsync(request.Id, cancellationToken);
        
        if (deleted)
        {
            // Publish domain event
            var domainEvent = new ProductDeletedEvent(request.Id, DateTime.UtcNow);
            await _mediator.Publish(domainEvent, cancellationToken);

            _logger.LogInformation("Product deleted successfully with ID: {ProductId}", request.Id);
        }
        else
        {
            _logger.LogWarning("Product not found for deletion with ID: {ProductId}", request.Id);
        }

        return deleted;
    }
}