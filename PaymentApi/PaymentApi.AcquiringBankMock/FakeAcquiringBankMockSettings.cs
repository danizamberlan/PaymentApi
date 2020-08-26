namespace PaymentApi.AcquiringBankMock
{
    using System;
    using Models;
    using Settings;

    public class FakeAcquiringBankMockSettings : AcquiringBankMockSettings
    {
        private readonly string DEFAULT_CARDHOLDER = "Daniela Zamberlan";

        public FakeAcquiringBankMockSettings()
        {
            SetupPostPaymentRequestMocks();
        }

        private void SetupPostPaymentRequestMocks()
        {
            // General Rules

            this.AddPostPaymentRequestMockWithDetails(
                request =>
                    DateTime.Today
                    >= new DateTime(
                        request.CardExpirationYear,
                        request.CardExpirationMonth,
                        1)
                        .AddMonths(1),
                request => new MockBankTransactionRequestResult
                {
                    StatusCode = "EXPIRED_CARD",
                    TransactionId = Guid.NewGuid()
                });

            this.AddPostPaymentRequestMockWithDetails(
                request => request.CardNumber < 1000000000000000,
                request =>
                    new MockBankTransactionRequestResult
                    {
                        StatusCode = "INVALID_CARDNUMBER",
                        TransactionId = Guid.NewGuid()
                    });

            this.AddPostPaymentRequestMockWithDetails(
                request => request.Amount <= 0,
                request => new MockBankTransactionRequestResult
                {
                    StatusCode = "INVALID_AMOUNT",
                    TransactionId = Guid.NewGuid()
                });

            this.AddPostPaymentRequestMockWithDetails(
                request => request.CVV < 100,
                request =>
                    new MockBankTransactionRequestResult
                    {
                        StatusCode = "INVALID_CVV",
                        TransactionId = Guid.NewGuid()
                    });

            this.AddPostPaymentRequestMockWithDetails(
                request => true,
                request =>
                    new MockBankTransactionRequestResult
                    {
                        StatusCode = "SUCCESS",
                        TransactionId = Guid.NewGuid()
                    });
        }

        private void AddPostPaymentRequestMockWithDetails(
            Func<MockBankTransactionRequestParameters, bool> predicate,
            Func<MockBankTransactionRequestParameters, MockBankTransactionRequestResult> result)
        {
            Func<MockBankTransactionRequestParameters, MockBankTransactionRequestResult> resultWithDetails =
                (request) =>
                {
                    var payment = result(request);

                    this.AddGetPaymentDetailsMock(
                        payment.TransactionId,
                        new MockBankTransactionDetails
                        {
                            Amount = request.Amount,
                            CardHolder = DEFAULT_CARDHOLDER,
                            CardNumber = $"************{request.CardNumber.ToString().PadLeft(16, '*').Substring(12, 4)}",
                            Currency = request.Currency,
                            PaymentStatus = payment.StatusCode,
                            TransactionId = payment.TransactionId
                        });

                    return payment;
                };

            this.AddPostPaymentRequestMock(
                predicate,
                resultWithDetails);
        }
    }
}
