using AutoMapper;
using Company.Template.Application.Commands;
using Company.Template.Application.Handlers;
using Company.Template.Domain.Entities;
using Company.Template.Domain.Repositories;
using Company.Template.Shared.DTOs;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Company.Template.UnitTests.Application;

public class CreateProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<CreateProductCommandHandler>> _mockLogger;
    private readonly CreateProductCommandHandler _handler;

    public CreateProductCommandHandlerTests()
    {
        _mockRepository = new Mock<IProductRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<CreateProductCommandHandler>>();
        _handler = new CreateProductCommandHandler(_mockRepository.Object, _mockMapper.Object, _mockMediator.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateProductAndReturnDto()
    {
        // Arrange
        var command = new CreateProductCommand("Test Product", "Test Description", 99.99m);
        var product = Product.Create(command.Name, command.Description, command.Price);
        var productDto = new ProductDto(product.Id, product.Name, product.Description, product.Price, product.CreatedAt, product.UpdatedAt);

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        _mockMapper.Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
            .Returns(productDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(command.Name);
        result.Description.Should().Be(command.Description);
        result.Price.Should().Be(command.Price);

        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockMediator.Verify(m => m.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}