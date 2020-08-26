namespace PaymentApi.ApiClient.Dto
{
    /// <summary>
    /// Wraps a result with a Notification design pattern;
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class ApiResult<TResult> 
    {
        public bool IsSuccess { get; private set; }

        public string ErrorMessage { get; private set; }

        public TResult Content { get; private set; }

        /// <summary>
        /// Builds a Success result.
        /// </summary>
        /// <param name="content">Result content.</param>
        /// <returns>Success result.</returns>
        public static ApiResult<TResult> Success(TResult content)
        {
            return new ApiResult<TResult>
            {
                Content = content,
                IsSuccess = true,
                ErrorMessage = string.Empty
            };
        }

        /// <summary>
        /// Builds a Failure result.
        /// </summary>
        /// <param name="errorMessage">Error message.</param>
        /// <returns>Failure result.</returns>
        public static ApiResult<TResult> Failure(string errorMessage)
        {
            return new ApiResult<TResult>
            {
                Content = default,
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
