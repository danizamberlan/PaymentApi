namespace PaymentApi.API.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Commands.CreatePayment;
    using Application.Queries.GetPaymentDetails;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Parameters;

    [ApiController]
    [Route("payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator mediator;

        public PaymentController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{paymentId}")]
        [ProducesResponseType(typeof(Domain.PaymentDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPaymentDetails(Guid paymentId)
        {
            var payment = await mediator.Send(new GetPaymentDetailsQueryParameters(paymentId));

            if (!payment.Success)
            {
                return NotFound(); 
            }

            return Ok(payment.Value.PaymentDetails);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePayment(CreatePaymentParameters parameters)
        {
            var createParameters = new CreatePaymentCommandParameters
            {
                Amount = parameters.Amount,
                CardExpirationMonth = parameters.CardExpirationMonth,
                CardExpirationYear = parameters.CardExpirationYear,
                CardNumber = parameters.CardNumber,
                Currency = parameters.Currency,
                CVV = parameters.CVV
            };

            var result = await mediator.Send(createParameters);

            if (!result.Success)
            {
                return BadRequest(result.Messages.FirstOrDefault()); 
            }

            return CreatedAtAction("GetPaymentDetails", new { paymentId = result.Value }, null);
        }
    }
}
