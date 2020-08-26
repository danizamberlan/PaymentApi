namespace PaymentApi.ComponentTests
{
    using System;
    using AcquiringBankMock.Models;
    using AcquiringBankMock.Settings;

    public class TestAcquiringBankMockSettings : AcquiringBankMockSettings
    {
        public TestAcquiringBankMockSettings()
        {
            SetupGetPaymentDetailsMocks();
            SetupPostPaymentRequestMocks();
        }

        private void SetupGetPaymentDetailsMocks()
        {
            this.AddGetPaymentDetailsMock(
                new Guid("c2d65973-c51b-88f4-8e09-2a5a20e61437"),
                new MockBankTransactionDetails
                {
                    Amount = 123,
                    CardNumber = "************5678",
                    Currency = "EUR",
                    CardHolder = "Daniela CardHolder",
                    PaymentStatus = "SUCCESS",
                    TransactionId = new Guid("c2d65973-c51b-88f4-8e09-2a5a20e61437")
                });

        }

        private void SetupPostPaymentRequestMocks()
        {
            this.AddPostPaymentRequestMock(
            request => true,
            request =>
                new MockBankTransactionRequestResult
                {
                    StatusCode = "SUCCESS",
                    TransactionId = new Guid("c2d65973-c51b-88f4-8e09-2a5a20e61437")
                });
        }
    }
}
