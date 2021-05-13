using NeighborHelpMobileClient.Services.Contracts;
using NeighborHelpModels.Models;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace NeighborHelpMobileClient.ViewModels
{
    public class NewOrderViewModel : BaseViewModel
    {
        #region fields

        private string product;
        private string productDescription;
        private string cost;
        private int selectedOrderTypeIndex;
        private double doubleCost;

        #endregion fields

        #region properties

        public IOrderStore OrderStore => DependencyService.Get<IOrderStore>();

        public ObservableCollection<string> OrderTypes { get; set; }

        public int SelectedTypeIndex
        {
            get => selectedOrderTypeIndex;
            set => SetProperty(ref selectedOrderTypeIndex, value);
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

        public Command SaveCommand { get; set; }
        public Command CancelCommand { get; set; }

        #endregion properties

        #region constructor

        public NewOrderViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            InitializeOrderValuesLists();
        }

        private void InitializeOrderValuesLists()
        {
            OrderTypes = new ObservableCollection<string>(NeighborHelpModels.Extentions.OrderExtention.GetAllTypes(null));
            SelectedTypeIndex = -1;
        }

        #endregion constructor

        #region methods

        private bool ValidateSave()
        {
            bool isNotEmptyFields =  !string.IsNullOrWhiteSpace(Product)
                && !string.IsNullOrWhiteSpace(ProductDescription)
                && !string.IsNullOrWhiteSpace(Cost)
                && OrderTypes.ElementAtOrDefault(SelectedTypeIndex) !=null;

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
                OrderType = OrderTypes.ElementAtOrDefault(SelectedTypeIndex),
                Cost = this.doubleCost
            };

            await OrderStore.AddItemAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        #endregion methods
    }
}
