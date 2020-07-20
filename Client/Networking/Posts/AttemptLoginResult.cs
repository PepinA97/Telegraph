using MyClient.Windows.ChatWindow;

namespace MyClient.Networking.Posts
{
    class AttemptLoginResult : IPost
    {
        public enum ResultType
        {
            Failure,
            TooManyAttempts,
            Success
        }

        public ResultType Result { get; set; }

        public void Execute(ViewModel viewModel)
        {
            return;
        }
    }
}
