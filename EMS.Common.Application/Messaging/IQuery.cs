using EMS.Common.Domain;
using MediatR;

namespace EMS.Common.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
