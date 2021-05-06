using NeighborHelpMobileClient.Services.Contracts;
using NeighborHelpMobileClient.Views;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace NeighborHelpMobileClient.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public IUserStore UserStore => DependencyService.Get<IUserStore>();

        private string errorText;

        public string Login { get; set; }
        public string Password { get; set; }

        public string ErrorText 
        { 
            get => errorText;
            set => SetProperty(ref errorText, value);
        }

        public Command LoginCommand { get; }

        public Command RegistrationCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            RegistrationCommand = new Command(OnRegistrationClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            if (ValidateInput())
            {
                try
                {
                    IsBusy = true;

                    if (await UserStore.LoginAsync(Login, Password))
                    {
                        await Shell.Current.GoToAsync($"//{nameof(OrdersPage)}");
                    }
                    else
                    {
                        ErrorText = "Login or password is incorrect.";
                    }
                }
                catch(Exception e)
                {
                    Debug.Fail(e.Message);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private async void OnRegistrationClicked(object o)
        {
            try
            {
                await Shell.Current.GoToAsync($"//{nameof(RegistrationPage)}");
            }
            catch(Exception e)
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
