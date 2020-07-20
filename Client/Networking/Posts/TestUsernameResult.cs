using MyClient.Windows.ChatWindow;

namespace MyClient.Networking.Posts
{
    class TestUsernameResult : IPost
    {
        public bool Success { get; set; }

        public void Execute(ViewModel viewModel)
        {
            if (Success)
            {
                viewModel.TestUsernameResult = true;
            }
            else
            {
                viewModel.TestUsernameResult = false;
            }
        }
    }
}
