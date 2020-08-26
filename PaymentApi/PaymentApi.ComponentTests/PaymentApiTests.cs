namespace PaymentApi.ComponentTests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using API.Parameters;
    using Domain;
    using FluentAssertions;
    using Xunit;

    public class PaymentApiTests : IClassFixture<TestFixture>
    {
        [Fact]
        public async Task Get_WithExistingPaymentId_ReturnsPaymentDetails()
        {
            // Arrange
            var expectedPayment = new PaymentDetails
            {
                Amount = 123,
                CardHolder = "Daniela CardHolder",
                CardNumber = "************5678",
                Currency = "EUR",
                PaymentId = new Guid("647b1143-25e6-b364-664d-ecbeb7a86387"),
                PaymentStatus = "SUCCESS"
            };

            // Act
            var path = "payment/647b1143-25e6-b364-664d-ecbeb7a86387";
            
            var response = await TestFixture.Client.GetAsync(path);

            // Assert
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PaymentDetails>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            result
                .Should()
                .BeEquivalentTo(expectedPayment);
        }

        [Fact]
        public async Task Get_WithNonExistingPaymentId_ReturnsNotFound()
        {
            // Act
            var path = "payment/7b454fb5-a4b0-4f86-93ce-43852b659b5c";

            var response = await TestFixture.Client.GetAsync(path);

            // Assert
            response
                .StatusCode
                .Should()
                .Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Post_PaymentWithCorrectInformation_Success()
        {
            // Arrange
            var payment = new CreatePaymentParameters
            {
                Amount = 123,
                CardExpirationMonth = 5,
                CardExpirationYear = 2021,
                CardNumber = 1234567812345678,
                Currency = "EUR",
                CVV = 123
            };

            // Act
            var path = "payment";
            var content = new StringContent(
                JsonSerializer.Serialize(payment), Encoding.UTF8, "application/json");

            var response = await TestFixture.Client.PostAsync(path, content);

            // Assert
            response
                .StatusCode
                .Should()
                .Be(HttpStatusCode.Created);

            response
                .Headers
                .Location
                .AbsolutePath
                .Should()
                .Be(@"/payment/647b1143-25e6-b364-664d-ecbeb7a86387");
        }

        [Fact]
        public async Task Post_PaymentWithInvalidAmount_BadRequest()
        {
            // Arrange
            var payment = new CreatePaymentParameters
            {
                Amount = 0,
                CardExpirationMonth = 5,
                CardExpirationYear = 2021,
                CardNumber = 1234567812345678,
                Currency = "EUR",
                CVV = 123
            };

            // Act
            var path = "payment";
            var content = new StringContent(
            JsonSerializer.Serialize(payment), Encoding.UTF8, "application/json");

            var response = await TestFixture.Client.PostAsync(path, content);

            // Assert
            response
                .StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_PaymentWithInvalidExpirationMonth_BadRequest()
        {
            // Arrange
            var payment = new CreatePaymentParameters
            {
                Amount = 547,
                CardExpirationMonth = 52,
                CardExpirationYear = 2021,
                CardNumber = 1234567812345678,
                Currency = "EUR",
                CVV = 123
            };

            // Act
            var path = "payment";
            var content = new StringContent(
            JsonSerializer.Serialize(payment), Encoding.UTF8, "application/json");

            var response = await TestFixture.Client.PostAsync(path, content);

            // Assert
            response
                .StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_PaymentWithInvalidCVV_BadRequest()
        {
            // Arrange
            var payment = new CreatePaymentParameters
            {
                Amount = 547,
                CardExpirationMonth = 10,
                CardExpirationYear = 2021,
                CardNumber = 1234567812345678,
                Currency = "EUR",
                CVV = 1234
            };

            // Act
            var path = "payment";
            var content = new StringContent(
            JsonSerializer.Serialize(payment), Encoding.UTF8, "application/json");

            var response = await TestFixture.Client.PostAsync(path, content);

            // Assert
            response
                .StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_PaymentWithInvalidCurrency_BadRequest()
        {
            // Arrange
            var payment = new CreatePaymentParameters
            {
                Amount = 547,
                CardExpirationMonth = 2,
                CardExpirationYear = 2021,
                CardNumber = 1234567812345678,
                Currency = "EURO",
                CVV = 123
            };

            // Act
            var path = "payment";
            var content = new StringContent(
            JsonSerializer.Serialize(payment), Encoding.UTF8, "application/json");

            var response = await TestFixture.Client.PostAsync(path, content);

            // Assert
            response
                .StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
        }
    }
}
