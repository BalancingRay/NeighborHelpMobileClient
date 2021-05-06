using NeighborHelpMobileClient.ViewModels;
using Xamarin.Forms;

namespace NeighborHelpMobileClient.Views
{
    public partial class OrderDetailPage : ContentPage
    {
        public OrderDetailPage()
        {
            InitializeComponent();
            BindingContext = new OrderDetailViewModel();
        }
    }
}