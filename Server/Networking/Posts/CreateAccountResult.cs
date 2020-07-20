namespace MyServer.Networking.Posts
{
    class CreateAccountResult : IPost
    {
        public enum ResultType
        {
            AlreadyExists,
            TooManyAttempts,
            Success
        }

        public ResultType Result { get; }

        public CreateAccountResult(ResultType result)
        {
            Result = result;
        }
    }
}
