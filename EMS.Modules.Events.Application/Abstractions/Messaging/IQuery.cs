using EMS.Modules.Events.Domain.Abstractions;
using MediatR;

namespace EMS.Modules.Events.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
