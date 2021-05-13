using NeighborHelpMobileClient.Services.Contracts;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NeighborHelpMobileClient.Services
{
    public class MessageBoxService : IMessageBoxService
    {
        private static Page CurrentMainPage;

        public MessageBoxService(Page mainPage)
        {
            CurrentMainPage = mainPage;
        }

        public async Task<bool> ConfirmAction(string title, string message)
        {
            var result = await CurrentMainPage.DisplayAlert(title, message, "OK", "Cancel");
            return result;
        }
    }
}
