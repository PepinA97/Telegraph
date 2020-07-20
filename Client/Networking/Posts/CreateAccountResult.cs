using MyClient.Windows.ChatWindow;

namespace MyClient.Networking.Posts
{
    class CreateAccountResult : IPost
    {
        public enum ResultType
        {
            AlreadyExists,
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
