using EMS.Common.Domain;

namespace EMS.Modules.Events.Domain.TicketTypes;
public static class TicketTypeErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound(
            "TicketTypes.NotFound",
            $"Ticket type with ID '{id}' was not found."
        );
}
