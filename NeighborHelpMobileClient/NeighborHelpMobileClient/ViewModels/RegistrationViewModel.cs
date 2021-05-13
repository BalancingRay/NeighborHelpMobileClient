using NeighborHelpMobileClient.Services.Contracts;
using NeighborHelpMobileClient.Views;
using NeighborHelpModels.Models;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace NeighborHelpMobileClient.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {

        private string errorText;

        public IUserStore UserStore => DependencyService.Get<IUserStore>();

        public Command RegistrationCommand { get; }

        public string Login { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
        public string UserName { get; set; }

        public string ProfileName { get; set; }
        public string ProfileAddress { get; set; }
        public string ProfilePhoneNumber { get; set; }

        public string ErrorText
        {
            get => errorText;
            set => SetProperty(ref errorText, value);
        }

        public RegistrationViewModel()
        {
            Title = "Registration";
            RegistrationCommand = new Command(OnRegistration);
        }

        public async void OnRegistration(object o)
        {
            if (ValidateInput())
            {
                var profile = new UserProfile()
                {
                    Name = ProfileName,
                    Address = ProfileAddress,
                    PhoneNumber = ProfilePhoneNumber
                };

                var newUser = new User()
                {
                    Login = Login,
                    Password = Password,
                    UserName = UserName,
                    Role = "user",
                    Profile = profile
                };

                try
                {
                    if (await UserStore.AddItemAsync(newUser))
                    {
                        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                    }
                    else
                    {
                        ErrorText = "It Can't register new user. Try to change Login.";
                    }
                }
                catch (Exception e)
                {
                    Debug.Fail(e.Message);
                }
            }
        }

        private bool ValidateInput()
        {
            ErrorText = string.Empty;

            if (string.IsNullOrWhiteSpace(Login))
            {
                AddErrorText("Login can't be empty.");
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                AddErrorText("Password can't be empty.");
            }
            else if (string.IsNullOrWhiteSpace(ConfirmedPassword))
            {
                AddErrorText("Please, confirm password.");
            }
            else if (ConfirmedPassword != Password)
            {
                AddErrorText($"Passwords don't equals.");
            }

            bool isValid = string.IsNullOrEmpty(ErrorText);

            return isValid;
        }

        private void AddErrorText(string newError)
        {
            if (string.IsNullOrWhiteSpace(newError))
            {
                return;
            }

            if (!string.IsNullOrEmpty(ErrorText))
            {
                ErrorText += Environment.NewLine;
            }
            ErrorText += newError;
        }
    }
}
