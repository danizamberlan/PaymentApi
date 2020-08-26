namespace PaymentApi.CrossCutting.Cqrs
{
    using Ether.Outcomes;
    using MediatR;

    public interface IQueryHandler<in TRequest, TResponse> :
        IRequestHandler<TRequest, IOutcome<TResponse>> where TRequest : IRequest<IOutcome<TResponse>>
    {
    }
}