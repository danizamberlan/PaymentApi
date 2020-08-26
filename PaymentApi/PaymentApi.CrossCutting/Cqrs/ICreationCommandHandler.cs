namespace PaymentApi.CrossCutting.Cqrs
{
    using System;
    using Ether.Outcomes;
    using MediatR;

    public interface ICreationCommandHandler<in TRequest> :
        IRequestHandler<TRequest, IOutcome<Guid>> where TRequest : ICreationCommand
    {
    }
}