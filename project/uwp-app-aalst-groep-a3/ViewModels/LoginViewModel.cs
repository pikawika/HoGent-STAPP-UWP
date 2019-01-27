using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Base;
using uwp_app_aalst_groep_a3.Network;
using uwp_app_aalst_groep_a3.Utils;
using Windows.Security.Credentials;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Controls;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private MainPageViewModel mainPageViewModel;
        private NetworkAPI networkAPI = new NetworkAPI();
        private PasswordVault passwordVault = new PasswordVault();

        public string Username { get; set; } = "";
        public string Password { get; set; } = "";

        public RelayCommand SignInCommand { get; set; }
        public RelayCommand NavigateToRegistrationCommand { get; set; }

        public LoginViewModel(MainPageViewModel mainPageViewModel)
        {
            this.mainPageViewModel = mainPageViewModel;
            SignInCommand = new RelayCommand(async _ => await SignInAsync());
            NavigateToRegistrationCommand = new RelayCommand(_ => NavigateToRegistration());
        }

        private async Task SignInAsync()
        {
            if (string.IsNullOrWhiteSpace(Username)
                || string.IsNullOrWhiteSpace(Password))
            {
                await MessageUtils.ShowDialog("Aanmelden", "Gelieve zowel uw gebruikersnaam als uw wachtwoord in te voeren.");
                return;
            }

            var token = await networkAPI.SignIn(Username, Password);

            if (string.IsNullOrWhiteSpace(token))
            {
                await MessageUtils.ShowDialog("Aanmelden", "Er is een fout opgetreden tijdens het aanmelden.");
                return;
            }

            passwordVault.Add(new PasswordCredential("Stapp", "Token", token));

            NavigateToAccount();
            mainPageViewModel.NavigationHistoryItems.RemoveAll(v => v.GetType() == typeof(LoginViewModel) || v.GetType() == typeof(RegistrationViewModel) || v.GetType() == typeof(MerchantRegistrationViewModel));

            var role = UserUtils.GetUserRole();
            if (role.ToLower() == "customer") mainPageViewModel.AddSubscriptionNavigationViewItem();
            else if (role.ToLower() == "merchant") mainPageViewModel.AddMerchantPanelNavigationViewItem();

            await MessageUtils.ShowDialog("Aanmelden", "Welkom bij Stapp!");
        }

        private void NavigateToRegistration() => mainPageViewModel.NavigateTo(new RegistrationViewModel(mainPageViewModel));

        private void NavigateToAccount() => mainPageViewModel.NavigateTo(new AccountViewModel(mainPageViewModel));
    }
}
