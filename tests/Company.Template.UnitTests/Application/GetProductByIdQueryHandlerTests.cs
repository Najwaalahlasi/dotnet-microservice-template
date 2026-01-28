using AutoMapper;
using Company.Template.Application.Handlers;
using Company.Template.Application.Queries;
using Company.Template.Domain.Entities;
using Company.Template.Domain.Repositories;
using Company.Template.Shared.DTOs;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Company.Template.UnitTests.Application;

public class GetProductByIdQueryHandlerTests
{
    private readonly Mock<IProductRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<GetProductByIdQueryHandler>> _mockLogger;
    private readonly GetProductByIdQueryHandler _handler;

    public GetProductByIdQueryHandlerTests()
    {
        _mockRepository = new Mock<IProductRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<GetProductByIdQueryHandler>>();
        _handler = new GetProductByIdQueryHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_WhenProductExists_ShouldReturnProductDto()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = Product.Create("Test Product", "Test Description", 99.99m);
        var productDto = new ProductDto(product.Id, product.Name, product.Description, product.Price, product.CreatedAt, product.UpdatedAt);
        var query = new GetProductByIdQuery(productId);

        _mockRepository.Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        _mockMapper.Setup(m => m.Map<ProductDto>(product))
            .Returns(productDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(productDto);
        _mockRepository.Verify(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenProductDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var query = new GetProductByIdQuery(productId);

        _mockRepository.Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _mockRepository.Verify(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()), Times.Once);
    }
}