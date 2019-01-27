using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Base;
using uwp_app_aalst_groep_a3.Models;
using uwp_app_aalst_groep_a3.Models.Domain;
using uwp_app_aalst_groep_a3.Network;
using uwp_app_aalst_groep_a3.Utils;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Controls;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class AccountViewModel : ViewModelBase
    {
        private MainPageViewModel mainPageViewModel;
        private NetworkAPI networkAPI = new NetworkAPI();
        private PasswordVault passwordVault = new PasswordVault();

        private User _user;

        public User User
        {
            get => _user;
            set { _user = value; RaisePropertyChanged(nameof(User)); }
        }

        private string _buttonText = "";

        public string ButtonText
        {
            get => _buttonText;
            set { _buttonText = value; RaisePropertyChanged(nameof(ButtonText)); }
        }

        public RelayCommand SignOutCommand { get; set; }
        public RelayCommand ShowSubscriptionsCommand { get; set; }

        public RelayCommand ChangeUsernameCommand { get; set; }
        public RelayCommand ChangePasswordCommand { get; set; }

        public AccountViewModel(MainPageViewModel mainPageViewModel)
        {
            this.mainPageViewModel = mainPageViewModel;

            User = new User { FirstName = "", LastName = "", Email = "", Login = new Login() { Username = "" } };

            SignOutCommand = new RelayCommand(async _ => await SignOutAsync());
            ShowSubscriptionsCommand = new RelayCommand(_ => ShowPageMatchingRole());

            ChangeUsernameCommand = new RelayCommand(async _ => await ChangeUsername());
            ChangePasswordCommand = new RelayCommand(async _ => await ChangePassword());

            GetUser();

            var role = UserUtils.GetUserRole();

            if (role.ToLower() == "customer") ButtonText = "Bekijk abonnementen";
            else if (role.ToLower() == "merchant") ButtonText = "Bekijk uw overzicht";
        }

        private async Task ChangeUsername()
        {
            var username = await InputTextDialogAsync("Kies een gebruikersnaam");
            if (username != "")
            {
                var message = await networkAPI.ChangeUsername(username);
                await MessageUtils.ShowDialog("Gebruikersnaam wijzigen", message);
            }
        }

        private async Task ChangePassword()
        {
            var password = await PasswordInputTextDialogAsync("Kies een nieuw wachtwoord");
            var repeat = await PasswordInputTextDialogAsync("Herhaal het nieuwe wachtwoord");
            if (password != "")
            {
                if (password != repeat) await MessageUtils.ShowDialog("Wachtwoord wijzigen", "De twee ingevoerde wachtwoorden komen niet overeen.");
                else if (password.Length < 6) await MessageUtils.ShowDialog("Wachtwoord wijzigen", "Uw wachtwoord moet minstens 6 karakters lang zijn.");
                else if (password.Length > 30) await MessageUtils.ShowDialog("Wachtwoord wijzigen", "Uw wachtwoord moet mag niet langer dan 30 karakters zijn.");
                else
                {
                    var message = await networkAPI.ChangePassword(password);
                    await MessageUtils.ShowDialog("Wachtwoord wijzigen", message);
                }
            }
        }

        private async Task<string> InputTextDialogAsync(string title)
        {
            TextBox inputTextBox = new TextBox();
            inputTextBox.AcceptsReturn = false;
            inputTextBox.Height = 32;
            ContentDialog dialog = new ContentDialog();
            dialog.Content = inputTextBox;
            dialog.Title = title;
            dialog.IsSecondaryButtonEnabled = true;
            dialog.PrimaryButtonText = "Oké";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.SecondaryButtonText = "Annuleren";
            if (await dialog.ShowAsync() == ContentDialogResult.Primary) return inputTextBox.Text;
            else return "";
        }

        private async Task<string> PasswordInputTextDialogAsync(string title)
        {
            PasswordBox passwordBox = new PasswordBox();
            passwordBox.Height = 32;
            ContentDialog dialog = new ContentDialog();
            dialog.Content = passwordBox;
            dialog.Title = title;
            dialog.IsSecondaryButtonEnabled = true;
            dialog.PrimaryButtonText = "Oké";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.SecondaryButtonText = "Annuleren";
            if (await dialog.ShowAsync() == ContentDialogResult.Primary) return passwordBox.Password;
            else return "";
        }

        private async void GetUser() => User = await networkAPI.GetUser();

        private async Task SignOutAsync()
        {
            NavigateToLogin();
            mainPageViewModel.NavigationHistoryItems.RemoveAll(v =>
                v.GetType() == typeof(AccountViewModel) ||
                v.GetType() == typeof(SubscriptionsViewModel) ||
                v.GetType() == typeof(MerchantPanelViewModel) ||
                v.GetType() == typeof(MerchantAddViewModel) ||
                v.GetType() == typeof(MerchantEditViewModel));

            var role = UserUtils.GetUserRole();
            if (role.ToLower() == "customer") mainPageViewModel.RemoveSubscriptionNavigationViewItem();
            else if (role.ToLower() == "merchant") mainPageViewModel.RemoveMerchantPanelNavigationViewItem();

            UserUtils.RemoveUserToken();
            
            await MessageUtils.ShowDialog("Afmelden", "U bent succesvol afgemeld.");
        }

        private void ShowPageMatchingRole()
        {
            var role = UserUtils.GetUserRole();

            if (role.ToLower() == "customer") ShowSubscriptions();
            else if (role.ToLower() == "merchant") ShowMerchantPanel();
        }

        private void NavigateToLogin() => mainPageViewModel.NavigateTo(new LoginViewModel(mainPageViewModel));

        private void ShowSubscriptions() => mainPageViewModel.NavigateTo(new SubscriptionsViewModel(mainPageViewModel));

        private void ShowMerchantPanel() => mainPageViewModel.NavigateTo(new MerchantPanelViewModel(mainPageViewModel));
    }
}
