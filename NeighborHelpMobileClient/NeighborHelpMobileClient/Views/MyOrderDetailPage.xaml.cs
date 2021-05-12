using NeighborHelpMobileClient.ViewModels;
using Xamarin.Forms;

namespace NeighborHelpMobileClient.Views
{
    public partial class MyOrderDetailPage : ContentPage
    {
        public MyOrderDetailPage()
        {
            InitializeComponent();
            BindingContext = new OrderDetailViewModel();
        }
    }
}