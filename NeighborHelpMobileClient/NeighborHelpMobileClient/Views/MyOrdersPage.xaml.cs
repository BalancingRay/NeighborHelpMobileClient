using NeighborHelpMobileClient.ViewModels;
using Xamarin.Forms;

namespace NeighborHelpMobileClient.Views
{
    public partial class MyOrdersPage : ContentPage
    {
        MyOrdersViewModel _viewModel;

        public MyOrdersPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new MyOrdersViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}