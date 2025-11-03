using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Tajan.OrderService.Application.Usecases.Order.Commands.AddBasketItem;
using Tajan.OrderService.Application.Contracts.ExternalServices;
using Tajan.OrderService.Domain.Entities.BasketAggregates.Repositories;

namespace OrderService.Unit.Tests;

public class AddItemHandlerTests
{
    [Fact]
    public async Task Handle_ProductExists_CallsRepository()
    {
        // Arrange
        var productService = new Mock<IProductService>();
        productService.Setup(p => p.GetProductsAsync())
            .ReturnsAsync(new List<Tajan.OrderService.Domain.ExtrenalModels.Product>
            {
                new Tajan.OrderService.Domain.ExtrenalModels.Product { Id = 42, Price = 10.5m, Name = "p" }
            });

        var basketRepo = new Mock<IBasketRepository>();


        var handler = new AddBasketItemCommandHandler(basketRepo.Object, productService.Object);

        var cmd = new AddBasketItemCommand(1, 42, 2);

        // Act
        await handler.Handle(cmd, CancellationToken.None);

        // Assert
        basketRepo.Verify(r => r.AddOrUpdateItemAsync(1, 42, 2, 10.5m), Times.Once);
    }

    [Fact]
    public async Task Handle_ProductMissing_ThrowsKeyNotFoundException()
    {
    var productService = new Mock<IProductService>();
    productService.Setup(p => p.GetProductsAsync()).ReturnsAsync((List<Tajan.OrderService.Domain.ExtrenalModels.Product>?)null);

        var basketRepo = new Mock<IBasketRepository>();

        var handler = new AddBasketItemCommandHandler(basketRepo.Object, productService.Object);

        var cmd = new AddBasketItemCommand(1, 999, 1);

        await Assert.ThrowsAsync<System.Collections.Generic.KeyNotFoundException>(() => handler.Handle(cmd, CancellationToken.None));
    }
}
