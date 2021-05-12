using NeighborHelpMobileClient.Services.Contracts;
using NeighborHelpModels.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NeighborHelpMobileClient.ViewModels.Base
{
    public abstract class OrderViewModelBase: BaseViewModel
    {
        #region Fields

        private string id;
        private string product;
        private string productDescription;
        private string cost;
        private string orderType;
        private string status;
        private string authorName;
        private string profilePhoneNumber;
        private string profileAddress;

        private bool isItemCommandVisible;
        private Command itemCommand;

        private Order loadedItem;
        protected Order LoadedItem => loadedItem;

        #endregion Fields

        #region Properties

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

                if (old != id)
                    LoadItemId(value);
            }
        }

        public string CommandName { get; protected set; }

        public bool IsItemCommandVisible
        {
            get => isItemCommandVisible;
            set => SetProperty(ref isItemCommandVisible, value);
        }
        public Command ItemCommand => itemCommand ?? (itemCommand = new Command(OnCommandExecute));

        protected IOrderStore OrderStore => DependencyService.Get<IOrderStore>();

        #endregion Properties

        #region Methods
        protected virtual async Task LoadItemId(string itemId)
        {
            try
            {
                loadedItem = null;
                var item = await OrderStore.GetItemAsync(itemId);

                if (item == null)
                    return;

                Product = item.Product;
                ProductDescription = item.ProductDescription;
                Cost = item.Cost.ToString();
                OrderType = item.OrderType;
                Status = item.Status;
                AuthorName = item.Author?.Name;
                AuthorId = item.AuthorId.ToString();
                ProfileAddress = item.Author?.Address;
                ProfilePhoneNumber = item.Author?.PhoneNumber;

                loadedItem = item;
            }
            catch (Exception exc)
            {
                Debug.Fail(exc.Message);
                Debug.WriteLine("Failed to Load Item");
            }
        }

        protected abstract void OnCommandExecute();

        #endregion Methods
    }
}
