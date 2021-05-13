using NeighborHelpMobileClient.Services.Contracts;
using NeighborHelpMobileClient.Views;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NeighborHelpMobileClient.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public IUserStore UserStore => DependencyService.Get<IUserStore>();

        private string errorText;
        private bool isLogined;
        private bool isUnlogined;
        private string login;
        private string password;
        public string Login
        {
            get => login;
            set => SetProperty(ref login, value);
        }
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }


        public string ErrorText
        {
            get => errorText;
            set => SetProperty(ref errorText, value);
        }

        public bool IsLogined
        {
            get => isLogined;
            set
            {
                SetProperty(ref isLogined, value);
                IsUnlogined = !value;
            }
        }

        public bool IsUnlogined
        {
            get => isUnlogined;
            set => SetProperty(ref isUnlogined, value);
        }

        public Command LoginCommand { get; }

        public Command UnloginCommand { get; }

        public Command RegistrationCommand { get; }

        public LoginViewModel()
        {
            IsLogined = false;
            LoginCommand = new Command( async ()=> await OnLoginClicked());
            UnloginCommand = new Command(async () => await UnregisteredClicked());
            RegistrationCommand = new Command(async ()=> await OnRegistrationClicked());
        }

        private async Task OnLoginClicked()
        {
            if (!ValidateInput())
                return;

            try
            {
                IsBusy = true;

                if (await UserStore.LoginAsync(Login, Password))
                {
                    IsLogined = true;
                    Password = string.Empty;
                    await Shell.Current.GoToAsync($"//{nameof(OrdersPage)}");
                }
                else
                {
                    ErrorText = "Login or password is incorrect.";
                }
            }
            catch (Exception e)
            {
                Debug.Fail(e.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnRegistrationClicked()
        {
            try
            {
                await Shell.Current.GoToAsync($"{nameof(RegistrationPage)}");
            }
            catch (Exception e)
            {
                Debug.Fail(e.Message);
            }
        }

        private async Task UnregisteredClicked()
        {
            try
            {
                UserStore.Unlogin();
                IsLogined = false;
            }
            catch (Exception e)
            {
                Debug.Fail(e.Message);
            }
        }

        private bool ValidateInput()
        {
            ErrorText = string.Empty;

            if (string.IsNullOrWhiteSpace(Login))
            {
                ErrorText += $"Login can't be empty.{Environment.NewLine}";
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorText += $"Password can't be empty.";
            }

            bool isValid = string.IsNullOrEmpty(ErrorText);

            return isValid;
        }
    }
}
