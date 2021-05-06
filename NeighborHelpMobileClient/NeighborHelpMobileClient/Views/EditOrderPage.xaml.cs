using NeighborHelpMobileClient.ViewModels;
using NeighborHelpModels.Models;
using Xamarin.Forms;

namespace NeighborHelpMobileClient.Views
{
    public partial class EditOrderPage : ContentPage
    {
        public Order Item { get; set; }

        public EditOrderPage()
        {
            InitializeComponent();
            BindingContext = new EditOrderViewModel();
        }
    }
}