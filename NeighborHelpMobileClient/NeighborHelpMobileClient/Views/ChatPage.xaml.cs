using NeighborHelpMobileClient.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NeighborHelpMobileClient.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage
    {
        ChatViewModel viewModel;
        public ChatPage()
        {
            InitializeComponent();
            viewModel = new ChatViewModel();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.Connect();
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            await viewModel.Disconnect();
        }
    }
}