using NeighborHelpMobileClient.Services.Contracts;
using NeighborHelpModels.Models;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace NeighborHelpMobileClient.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class EditOrderViewModel : BaseViewModel
    {
        public IOrderStore OrderStore => DependencyService.Get<IOrderStore>();

        private string id;
        private string product;
        private string productDescription;
        private string cost;
        private string orderType;
        private double doubleCost;
        private string status;
        private int authorId;

        public EditOrderViewModel()
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
                && !string.IsNullOrWhiteSpace(OrderType)
                && !string.IsNullOrWhiteSpace(id);

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
        public string Status
        {
            get => status;
            set => SetProperty(ref status, value);
        }

        public string ItemId
        {
            get => id;
            set
            {
                string old = id;
                id = value;

                if (old != id)
                    LoadItemId(value);
            }
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
                Id = int.Parse(this.id),
                AuthorId = this.authorId,
                Product = this.Product,
                ProductDescription = this.ProductDescription,
                OrderType = this.OrderType,
                Cost = this.doubleCost,
                Status = this.Status
            };

            await OrderStore.UpdateItemAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await OrderStore.GetItemAsync(itemId);
                Product = item.Product;
                ProductDescription = item.ProductDescription;
                Cost = item.Cost.ToString();
                OrderType = item.OrderType;
                Status = item.Status;
                authorId = item.AuthorId;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
