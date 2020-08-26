namespace PaymentApi.AcquiringBankMock.Settings
{
    using System;
    using System.Collections.Generic;
    using Models;

    /// <summary>
    /// Settings to be consumed by the Acquire Bank mock Controller.
    /// </summary>
    public abstract class AcquiringBankMockSettings
    {
        private readonly List<MockPattern<Guid, MockBankTransactionDetails>> getPaymentDetails
            = new List<MockPattern<Guid, MockBankTransactionDetails>>();

        private readonly List<MockPattern<
                                MockBankTransactionRequestParameters,
                                MockBankTransactionRequestResult>> postPaymentRequest
            = new List<MockPattern<MockBankTransactionRequestParameters, MockBankTransactionRequestResult>>();

        /// <summary>
        /// Adds a GetPaymentDetails predicate mapping between a given identifier and an instance of MockBankTransactionDetails. 
        /// </summary>
        /// <param name="id">Id to be received by the Api.</param>
        /// <param name="details">Instance to be returned.</param>
        public void AddGetPaymentDetailsMock(Guid id, MockBankTransactionDetails details)
        {
            getPaymentDetails.Add(
                new MockPattern<Guid, MockBankTransactionDetails>(
                    guid => guid == id,
                    guid => details));
        }

        /// <summary>
        /// Adds a PostPaymentRequest predicate to be matched against the posted parameters,
        /// as well as the factory method for building the response based on these parameters.
        /// </summary>
        /// <param name="predicate">Predicate to be matched against the posted parameters.</param>
        /// <param name="result">Factory method for building the response based on the parameters</param>
        public void AddPostPaymentRequestMock(
            Func<MockBankTransactionRequestParameters, bool> predicate,
            Func<MockBankTransactionRequestParameters, MockBankTransactionRequestResult> result)
        {
            postPaymentRequest.Add(
                new MockPattern<MockBankTransactionRequestParameters, MockBankTransactionRequestResult>(
                   predicate,
                   result));
        }

        /// <summary>
        /// Iterates over the previously registered GetPaymentDetails patterns attempting to match and produce a response.
        /// Returns the first matching pattern. In case no matches are found, returns null.
        /// </summary>
        /// <param name="guid">Id provided by the Api.</param>
        /// <returns>Transaction details produced.</returns>
        public MockBankTransactionDetails ProcessGetPaymentDetailsMock(Guid guid)
        {
            return Process(getPaymentDetails, guid);
        }

        /// <summary>
        /// Iterates over the previously registered PostPaymentRequest patterns attempting to match and produce a response.
        /// Returns the first matching pattern. In case no matches are found, returns null.
        /// </summary>
        /// <param name="parameters">Parameters provided by the Api.</param>
        /// <returns>Transaction request result produced.</returns>
        public MockBankTransactionRequestResult ProcessPostPaymentRequestMock(MockBankTransactionRequestParameters parameters)
        {
            return Process(postPaymentRequest, parameters);
        }

        private TResult Process<TRequest, TResult>(IEnumerable<MockPattern<TRequest, TResult>> mocks, TRequest request) where TResult : class
        {
            foreach (var pattern in mocks)
            {
                if (pattern.Predicate(request))
                {
                    return pattern.Result(request);
                }
            }

            return null;
        }
    }
}