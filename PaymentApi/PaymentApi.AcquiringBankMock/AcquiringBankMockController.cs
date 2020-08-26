namespace PaymentApi.AcquiringBankMock
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Settings;

    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/acquiringBankMock")]
    public class AcquiringBankMockController : ControllerBase
    {
        private AcquiringBankMockSettings mockSettings { get; }

        public AcquiringBankMockController(AcquiringBankMockSettings mockSettings)
        {
            this.mockSettings = mockSettings;
        }

        [Route("payments/{id}")]
        [HttpGet]
        public IActionResult Get(Guid id)
        {
            var result = mockSettings.ProcessGetPaymentDetailsMock(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [Route("payments")]
        [HttpPost]
        public IActionResult Post(
            [FromBody]MockBankTransactionRequestParameters parameters)
        {
            var result = mockSettings.ProcessPostPaymentRequestMock(parameters);

            if (result == null)
            {
                return StatusCode(500);
            }

            return Ok(result);
        }
    }
}
