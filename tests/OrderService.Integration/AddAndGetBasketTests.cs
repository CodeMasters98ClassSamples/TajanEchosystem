using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Tajan.OrderService.Infrastructure.Persistence.Contexts;
using Tajan.OrderService.Infrastructure.Persistence.Repositories;
using Tajan.OrderService.Domain.Entities.BasketAggregates.Repositories;
using Tajan.OrderService.Application.Usecases.Order.Commands.AddBasketItem;
using Tajan.OrderService.Application.Usecases.Order.Queries.GetBasket;
using Tajan.OrderService.Application.Contracts.ExternalServices;
using MediatR;
using System.Collections.Generic;

namespace OrderService.Integration.Tests;

public class AddAndGetBasketTests
{
    [Fact]
    public async Task AddItemThenGetBasket_ReturnsItem()
    {
        var services = new ServiceCollection();

        services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseInMemoryDatabase("test-db"));

    services.AddScoped<IBasketRepository, BasketRepository>();

        // lightweight product stub
        services.AddSingleton<IProductService>(new InMemoryProductService());

    services.AddLogging();
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddBasketItemCommandHandler).Assembly));

        var provider = services.BuildServiceProvider();

        // seed nothing, run handler
        var mediator = provider.GetRequiredService<IMediator>();

    var addCmd = new AddBasketItemCommand(10, 42, 3);
    await mediator.Send(addCmd);

    var query = new GetBasketQuery(10);
    var result = await mediator.Send(query);

        Assert.NotNull(result);
        Assert.Equal(10, result.UserId);
        Assert.Single(result.Items);
        var item = result.Items.First();
        Assert.Equal(42, item.ProductId);
        Assert.Equal(3, item.Quantity);
    }

    class InMemoryProductService : IProductService
    {
        public Task<List<Tajan.OrderService.Domain.ExtrenalModels.Product>?> GetProductsAsync()
        {
            var list = new List<Tajan.OrderService.Domain.ExtrenalModels.Product>
            {
                new Tajan.OrderService.Domain.ExtrenalModels.Product { Id = 42, Price = 12.5m, Name = "p" }
            };
            return Task.FromResult((List<Tajan.OrderService.Domain.ExtrenalModels.Product>?)list);
        }
    }
}
