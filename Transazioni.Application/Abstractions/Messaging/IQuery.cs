using Transazioni.Domain.Abstractions;
using MediatR;

namespace Transazioni.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}