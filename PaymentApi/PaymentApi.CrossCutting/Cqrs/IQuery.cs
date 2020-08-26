namespace PaymentApi.CrossCutting.Cqrs
{
    using Ether.Outcomes;
    using MediatR;

    public interface IQuery<TResponse> : IRequest<IOutcome<TResponse>>
    {
    }
}
