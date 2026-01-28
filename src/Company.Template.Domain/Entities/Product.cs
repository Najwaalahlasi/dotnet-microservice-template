using Company.Template.Domain.Common;

namespace Company.Template.Domain.Entities;

/// <summary>
/// Represents a product entity in the domain
/// </summary>
public class Product : BaseEntity
{
    /// <summary>
    /// Gets or sets the product name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product price
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Creates a new product instance
    /// </summary>
    /// <param name="name">Product name</param>
    /// <param name="description">Product description</param>
    /// <param name="price">Product price</param>
    /// <returns>New product instance</returns>
    public static Product Create(string name, string description, decimal price)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Price = price,
            CreatedAt = DateTime.UtcNow
        };

        return product;
    }

    /// <summary>
    /// Updates the product information
    /// </summary>
    /// <param name="name">New product name</param>
    /// <param name="description">New product description</param>
    /// <param name="price">New product price</param>
    public void Update(string name, string description, decimal price)
    {
        Name = name;
        Description = description;
        Price = price;
        UpdatedAt = DateTime.UtcNow;
    }
}