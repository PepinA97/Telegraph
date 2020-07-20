namespace MyServer.Networking.Posts
{
    class AttemptLoginResult : IPost
    {
        public enum ResultType
        {
            Failure,
            TooManyAttempts,
            Success
        }

        public ResultType Result { get; }

        public AttemptLoginResult(ResultType result)
        {
            Result = result;
        }
    }
}
