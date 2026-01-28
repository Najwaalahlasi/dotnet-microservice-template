using Company.Template.Domain.Entities;
using Company.Template.Domain.Repositories;
using Company.Template.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Company.Template.Infrastructure.Repositories;

/// <summary>
/// Implementation of IProductRepository using Entity Framework Core
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(ApplicationDbContext context, ILogger<ProductRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Getting product by ID: {ProductId}", id);
        return await _context.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(int skip = 0, int take = 100, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Getting products with skip: {Skip}, take: {Take}", skip, take);
        
        return await _context.Products
            .OrderBy(p => p.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Getting total product count");
        return await _context.Products.CountAsync(cancellationToken);
    }

    public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Adding product with ID: {ProductId}", product.Id);
        
        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);
        
        return product;
    }

    public async Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Updating product with ID: {ProductId}", product.Id);
        
        _context.Products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
        
        return product;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Deleting product with ID: {ProductId}", id);
        
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (product == null)
        {
            return false;
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}