using NeighborHelpMobileClient.Services.Contracts;
using NeighborHelpModels.Models;
using Xamarin.Forms;

namespace NeighborHelpMobileClient.ViewModels
{
    public class NewOrderViewModel : BaseViewModel
    {
        public IOrderStore OrderStore => DependencyService.Get<IOrderStore>();

        private string product;
        private string productDescription;
        private string cost;
        private string orderType;
        private double doubleCost;

        public NewOrderViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            bool isNotEmptyFields =  !string.IsNullOrWhiteSpace(Product)
                && !string.IsNullOrWhiteSpace(ProductDescription)
                && !string.IsNullOrWhiteSpace(Cost)
                && !string.IsNullOrWhiteSpace(OrderType);

            if(isNotEmptyFields && double.TryParse(Cost, out double result))
            {
                doubleCost = result;
            }
            else
            {
                return false;
            }

            return true;
        }

        public string Product
        {
            get => product;
            set => SetProperty(ref product, value);
        }
        public string ProductDescription
        {
            get => productDescription;
            set => SetProperty(ref productDescription, value);
        }
        public string Cost
        {
            get => cost;
            set => SetProperty(ref cost, value);
        }
        public string OrderType
        {
            get => orderType;
            set => SetProperty(ref orderType, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Order newItem = new Order()
            {
                Product = this.Product,
                ProductDescription = this.ProductDescription,
                OrderType = this.OrderType,
                Cost = this.doubleCost
            };

            await OrderStore.AddItemAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
