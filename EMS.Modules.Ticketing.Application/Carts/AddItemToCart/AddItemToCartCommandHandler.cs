using EMS.Common.Application.Messaging;
using EMS.Common.Domain;
using EMS.Modules.Events.PublicApi;
using EMS.Modules.Ticketing.Domain.Customers;
using EMS.Modules.Ticketing.Domain.Events;

namespace EMS.Modules.Ticketing.Application.Carts.AddItemToCart;

internal sealed class AddItemToCartCommandHandler(
    CartService cartService,
    ICustomerRepository customerRepository,
    IEventsApi eventsApi)
    : ICommandHandler<AddItemToCartCommand>
{
    public async Task<Result> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        // 1. Get Customer
        Customer? customer = await customerRepository.GetAsync(request.CustomerId, cancellationToken);

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

