﻿using EMS.Common.Application.Messaging;
using EMS.Common.Domain;
using EMS.Modules.Events.PublicApi;
using EMS.Modules.Ticketing.Domain.Customers;
using EMS.Modules.Ticketing.Domain.Events;
using EMS.Modules.Users.PublicApi;
using FluentValidation;

namespace EMS.Modules.Ticketing.Application.Carts.AddItemToCart;
public sealed record AddItemToCartCommand(Guid CustomerId, Guid TicketTypeId, decimal Quantity) : ICommand;

internal sealed class AddItemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
{
    public AddItemToCartCommandValidator()
    {
        RuleFor(c => c.CustomerId).NotEmpty();
        RuleFor(c => c.TicketTypeId).NotEmpty();
        RuleFor(c => c.Quantity).GreaterThan(decimal.Zero);
    }
}

internal sealed class AddItemToCartCommandHandler(CartService cartService, IUsersApi usersApi, IEventsApi eventsApi)
    : ICommandHandler<AddItemToCartCommand>
{
    public async Task<Result> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        // 1. Get Customer
        UserResponse? customer = await usersApi.GetAsync(request.CustomerId, cancellationToken);

        if (customer is null)
        {
            return Result.Failure(CustomerErrors.NotFound(request.CustomerId));
        }

        // 2. Get ticket type
        TicketTypeResponse? ticketType = await eventsApi.GetTicketTypeAsync(request.TicketTypeId, cancellationToken);

        if (ticketType is null)
        {
            return Result.Failure(TicketTypeErrors.NotFound(request.TicketTypeId));
        }

        // 3. Add item to cart
        var cartItem = new CartItem
        {
            TicketTypeId = ticketType.Id,
            Price = ticketType.Price,
            Quantity = request.Quantity,
            Currency = ticketType.Currency,
        };

        await cartService.AddItemAsync(customer.Id, cartItem, cancellationToken);

        return Result.Success();
    }
}

