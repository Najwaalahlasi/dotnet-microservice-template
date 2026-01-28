using Company.Template.Infrastructure.Data;
using Company.Template.Shared.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace Company.Template.IntegrationTests.Controllers;

public class ProductsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public ProductsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove the real database context
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add in-memory database for testing
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnCreatedProduct()
    {
        // Arrange
        var createProductDto = new CreateProductDto("Test Product", "Test Description", 99.99m);

        // Act
        var response = await _client.PostAsJsonAsync("/api/products", createProductDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var content = await response.Content.ReadAsStringAsync();
        var productDto = JsonSerializer.Deserialize<ProductDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        productDto.Should().NotBeNull();
        productDto!.Name.Should().Be(createProductDto.Name);
        productDto.Description.Should().Be(createProductDto.Description);
        productDto.Price.Should().Be(createProductDto.Price);
        productDto.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetProduct_WhenProductExists_ShouldReturnProduct()
    {
        // Arrange - Create a product first
        var createProductDto = new CreateProductDto("Test Product", "Test Description", 99.99m);
        var createResponse = await _client.PostAsJsonAsync("/api/products", createProductDto);
        var createdContent = await createResponse.Content.ReadAsStringAsync();
        var createdProduct = JsonSerializer.Deserialize<ProductDto>(createdContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Act
        var response = await _client.GetAsync($"/api/products/{createdProduct!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        var productDto = JsonSerializer.Deserialize<ProductDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        productDto.Should().NotBeNull();
        productDto!.Id.Should().Be(createdProduct.Id);
        productDto.Name.Should().Be(createProductDto.Name);
    }

    [Fact]
    public async Task GetProduct_WhenProductDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/products/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ListProducts_ShouldReturnPagedResponse()
    {
        // Arrange - Create some products first
        var createProductDto1 = new CreateProductDto("Product 1", "Description 1", 10.00m);
        var createProductDto2 = new CreateProductDto("Product 2", "Description 2", 20.00m);
        
        await _client.PostAsJsonAsync("/api/products", createProductDto1);
        await _client.PostAsJsonAsync("/api/products", createProductDto2);

        // Act
        var response = await _client.GetAsync("/api/products?pageNumber=1&pageSize=10");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        var pagedResponse = JsonSerializer.Deserialize<ProductsPagedResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        pagedResponse.Should().NotBeNull();
        pagedResponse!.Products.Should().HaveCountGreaterOrEqualTo(2);
        pagedResponse.TotalCount.Should().BeGreaterOrEqualTo(2);
    }
}