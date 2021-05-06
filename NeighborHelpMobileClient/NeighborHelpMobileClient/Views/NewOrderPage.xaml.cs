using NeighborHelpMobileClient.ViewModels;
using NeighborHelpModels.Models;
using Xamarin.Forms;

namespace NeighborHelpMobileClient.Views
{
    public partial class NewOrderPage : ContentPage
    {
        public Order Item { get; set; }

        public NewOrderPage()
        {
            InitializeComponent();
            BindingContext = new NewOrderViewModel();
        }
    }
}