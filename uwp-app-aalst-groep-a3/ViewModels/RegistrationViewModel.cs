using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Base;
using uwp_app_aalst_groep_a3.Network;
using uwp_app_aalst_groep_a3.Utils;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Controls;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class RegistrationViewModel : ViewModelBase
    {
        private MainPageViewModel mainPageViewModel;
        private NetworkAPI networkAPI = new NetworkAPI();

        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string EmailAddress { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string RepeatPassword { get; set; } = "";

        public RelayCommand RegisterCommand { get; set; }
        public RelayCommand NavigateToLoginCommand { get; set; }

        public RegistrationViewModel(MainPageViewModel mainPageViewModel)
        {
            this.mainPageViewModel = mainPageViewModel;

            RegisterCommand = new RelayCommand(async _ => await CreateAccountAsync());
            NavigateToLoginCommand = new RelayCommand(_ => NavigateToLogin());
        }

        private async Task CreateAccountAsync()
        {
            if (string.IsNullOrWhiteSpace(FirstName)
                || string.IsNullOrWhiteSpace(LastName)
                || string.IsNullOrWhiteSpace(EmailAddress)
                || string.IsNullOrWhiteSpace(Username)
                || string.IsNullOrWhiteSpace(Password)
                || string.IsNullOrWhiteSpace(RepeatPassword))
            {
                await ShowDialog("Account aanmaken", "Gelieve in ieder veld een waarde in te voeren.");
                return;
            }

            if (!Regex.IsMatch(EmailAddress, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
            {
                await ShowDialog("Account aanmaken", "Gelieve een geldig e-mailadres in te voeren.");
                return;
            }

            if (Password != RepeatPassword)
            {
                await ShowDialog("Account aanmaken", "Wachtwoord en herhaal wachtwoord komen niet overeen.");
                return;
            }

            var token = await networkAPI.CreateAccount(FirstName, LastName, EmailAddress, Username, Password);

            if (string.IsNullOrWhiteSpace(token))
            {
                await ShowDialog("Account aanmaken", "Er is een fout opgetreden tijdens het aanmaken van een account.");
                return;
            }

            NavigateToLogin();
            await ShowDialog("Account aanmaken", "Uw account werd succesvol aangemaakt. Welkom bij Stapp!");
        }

        private async Task ShowDialog(string title, string message)
        {
            ContentDialog contentDialog = new ContentDialog();

            contentDialog.Title = title;
            contentDialog.Content = message;
            contentDialog.PrimaryButtonText = "Oké";

            await contentDialog.ShowAsync();
        }

        private void NavigateToLogin() => mainPageViewModel.NavigateTo(new LoginViewModel(mainPageViewModel));
    }
}
