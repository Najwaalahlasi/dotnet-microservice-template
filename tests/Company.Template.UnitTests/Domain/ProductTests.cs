using Company.Template.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Company.Template.UnitTests.Domain;

public class ProductTests
{
    [Fact]
    public void Create_ShouldCreateProductWithCorrectProperties()
    {
        // Arrange
        var name = "Test Product";
        var description = "Test Description";
        var price = 99.99m;

        // Act
        var product = Product.Create(name, description, price);

        // Assert
        product.Should().NotBeNull();
        product.Id.Should().NotBeEmpty();
        product.Name.Should().Be(name);
        product.Description.Should().Be(description);
        product.Price.Should().Be(price);
        product.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        product.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void Update_ShouldUpdateProductProperties()
    {
        // Arrange
        var product = Product.Create("Original Name", "Original Description", 50.00m);
        var newName = "Updated Name";
        var newDescription = "Updated Description";
        var newPrice = 75.00m;

        // Act
        product.Update(newName, newDescription, newPrice);

        // Assert
        product.Name.Should().Be(newName);
        product.Description.Should().Be(newDescription);
        product.Price.Should().Be(newPrice);
        product.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}