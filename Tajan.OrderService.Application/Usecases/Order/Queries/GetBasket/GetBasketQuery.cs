using MediatR;
using Tajan.OrderService.Application.Dtos;

namespace Tajan.OrderService.Application.Usecases.Order.Queries.GetBasket;

public record GetBasketQuery(int UserId) : IRequest<BasketDto?>;
