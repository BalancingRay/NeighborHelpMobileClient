using NeighborHelpMobileClient.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace NeighborHelpMobileClient.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}