namespace PaymentApi.AcquiringBankMock.Settings
{
    using System;

    /// <summary>
    /// This class encapsulate a predicate and a factory method to be used if that predicate gets matched against a request.
    /// </summary>
    /// <typeparam name="TRequest">Request type. Determines the type of the input parameter of the predicate.</typeparam>
    /// <typeparam name="TResult">Result type. Determines the type of the object built by the factory method.</typeparam>
    public class MockPattern<TRequest, TResult>
    {
        public Func<TRequest, bool> Predicate { get; }

        public Func<TRequest, TResult> Result { get; }

        public MockPattern(
            Func<TRequest, bool> predicate,
            Func<TRequest, TResult> result)
        {
            Predicate = predicate;
            Result = result;
        }
    }
}