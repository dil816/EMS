using EMS.Common.Application.Messaging;
using EMS.Common.Domain;
using EMS.Modules.Ticketing.Domain.Customers;
using EMS.Modules.Ticketing.Domain.Events;

namespace EMS.Modules.Ticketing.Application.Carts.AddItemToCart;

internal sealed class AddItemToCartCommandHandler(
    CartService cartService,
    ICustomerRepository customerRepository,
    ITicketTypeRepository ticketTypeRepository)
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
        TicketType? ticketType = await ticketTypeRepository.GetAsync(request.TicketTypeId, cancellationToken);

        if (ticketType is null)
        {
            return Result.Failure(TicketTypeErrors.NotFound(request.TicketTypeId));
        }

        if (ticketType.AvailableQuantity < request.Quantity)
        {
            return Result.Failure(TicketTypeErrors.NotEnoughQuantity(ticketType.AvailableQuantity));
        }

        // 3. Add item to cart
        var cartItem = new CartItem
        {
            TicketTypeId = request.TicketTypeId,
            Quantity = request.Quantity,
            Price = ticketType.Price,
            Currency = ticketType.Currency
        };

        await cartService.AddItemAsync(request.CustomerId, cartItem, cancellationToken);

        return Result.Success();
    }
}

