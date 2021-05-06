using NeighborHelpMobileClient.Services.Contracts;
using NeighborHelpMobileClient.Views;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace NeighborHelpMobileClient.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class OrderDetailViewModel : BaseViewModel
    {
        public IOrderStore OrderStore => DependencyService.Get<IOrderStore>();

        private string id;
        private string product;
        private string productDescription;
        private string cost;
        private string orderType;
        private string status;
        private string authorName;
        private string profilePhoneNumber;
        private string profileAddress;

        public string AuthorId { get; set; }
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

        public string AuthorName
        {
            get => authorName;
            set => SetProperty(ref authorName, value);
        }

        public string ProfileAddress
        {
            get => profileAddress;
            set => SetProperty(ref profileAddress, value);
        }

        public string ProfilePhoneNumber
        {
            get => profilePhoneNumber;
            set => SetProperty(ref profilePhoneNumber, value);
        }


        public string ItemId
        {
            get => id;
            set
            {
                string old = id;
                id = value;

                if(old!=id)
                    LoadItemId(value);
            }
        }

        private Command editItemCommand;

        public Command EditItemCommand => editItemCommand ?? (editItemCommand = new Command(OnEdit));

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
                AuthorName = item.Author?.Name;
                AuthorId = item.AuthorId.ToString();
                ProfileAddress = item.Author?.Address;
                ProfilePhoneNumber = item.Author?.PhoneNumber;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        async void OnEdit()
        {
            if (string.IsNullOrEmpty(id))
                return;
            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(EditOrderPage)}?{nameof(EditOrderViewModel.ItemId)}={id}");
        }
    }
}
