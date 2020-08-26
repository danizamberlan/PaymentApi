namespace PaymentApi.CrossCutting.Cqrs
{
    using System;
    using Ether.Outcomes;
    using MediatR;

    public interface ICreationCommand : IRequest<IOutcome<Guid>> 
    {
    }
}
