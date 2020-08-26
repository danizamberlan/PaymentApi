namespace PaymentApi.ComponentTests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit;

    public class ApiClientTests : IClassFixture<TestFixture>
    {
        [Fact]
        public async Task ApiClient_PostThenGet_RetrievesInformation()
        {
            // Arrange
            var payment = new ApiClient.Dto.CreatePaymentParameters
            {
                Amount = 123,
                CardExpirationMonth = 5,
                CardExpirationYear = 2021,
                CardNumber = 1234567812345678,
                Currency = "EUR",
                CVV = 123
            };

            var apiClient = new ApiClient.PaymentApiClient(
                                TestFixture.Client, 
                                "http://paymentapitestserver.com");

            // Act
            var response = await apiClient.CreatePaymentRequest(payment);

            response
                .IsSuccess
                .Should()
                .BeTrue();

            var resultPaymentDetails = await apiClient.GetPaymentTransactionDetail(response.Content);

            // Assert
            resultPaymentDetails
                .Should()
                .BeEquivalentTo(
                    payment, 
                    opt => 
                        opt.ExcludingMissingMembers()
                           .Excluding(p => p.CardNumber));

            resultPaymentDetails
                .IsSuccess
                .Should()
                .BeTrue();

            resultPaymentDetails
                .Content
                .CardNumber
                .Should()
                .Be("************5678");
        }
    }
}
