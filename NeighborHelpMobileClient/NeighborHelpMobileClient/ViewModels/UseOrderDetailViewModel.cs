using NeighborHelpMobileClient.Services.Contracts;
using NeighborHelpMobileClient.ViewModels.Base;
using NeighborHelpModels.Models;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NeighborHelpMobileClient.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class UseOrderDetailViewModel : OrderViewModelBase
    {
        private string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage;
            set => SetProperty(ref errorMessage, value);
        }

        public UseOrderDetailViewModel()
        {
            Title = "Order Details";
            IsItemCommandVisible = true;
            CommandName = "Apply";
        }

        protected override async void OnCommandExecute()
        {
            if (string.IsNullOrEmpty(ItemId))
                return;

            //TODO confirm action

            if (await OrderStore.UseOrder(BuildOrder()))
            {
                await LoadItemId(ItemId);
            }
            else
            {
                //TODO show messare
                ErrorMessage = "Attempt failed";
            }
        }

        private Order BuildOrder()
        {
            //var order = new Order()
            //{
            //    Id = LoadedItem.Id,
            //    Product = this.Product,
            //    ProductDescription = this.ProductDescription,
            //    Cost = double.Parse(this.Cost),
            //    OrderType = this.OrderType,
            //    Status = this.Status,
            //    AuthorId = int.Parse(this.AuthorId),
            //    Author = new UserProfile()
            //    {
            //        Name = AuthorName,
            //        Address = ProfileAddress,
            //        PhoneNumber = ProfilePhoneNumber
            //    }
            //};

            return LoadedItem;
        }

        protected override async Task LoadItemId(string itemId)
        {
            await base.LoadItemId(itemId);

            if (LoadedItem == null)
            {
                IsItemCommandVisible = false;
            }
            else
            {
                string clientId = LoadedItem.ClientId.ToString();
                string currentUserId = DependencyService.Get<IConnectorProvider>().GetToken()?.UserId;
                bool isItemHasActiveState = Status == NeighborHelpModels.Models.Consts.OrderStatus.ACTIVE;

                IsItemCommandVisible = currentUserId != null
                    && currentUserId != clientId
                    && isItemHasActiveState;
            }
        }
    }
}
