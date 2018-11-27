using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Network;
using uwp_app_aalst_groep_a3.Utils;
using Windows.UI.Xaml.Controls;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private MainPageViewModel mainPageViewModel;
        private NetworkAPI networkAPI = new NetworkAPI();

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
                ShowDialog("Aanmelding", "Gelieve zowel uw gebruikersnaam als uw wachtwoord in te voeren.");
                return;
            }

            var token = await networkAPI.SignIn(Username, Password);

            if (string.IsNullOrWhiteSpace(token))
            {
                ShowDialog("Aanmelding", "Er is een fout opgetreden tijdens het aanmelden.");
                return;
            }
            else
            {
                ShowDialog("Aanmelding", $"Tis toch simpel he: {token}");
            }
        }

        private async void ShowDialog(string title, string message)
        {
            ContentDialog contentDialog = new ContentDialog();

            contentDialog.Title = title;
            contentDialog.Content = message;
            contentDialog.PrimaryButtonText = "Oké";

            await contentDialog.ShowAsync();
        }

        private void NavigateToRegistration() => mainPageViewModel.CurrentData = new RegistrationViewModel(mainPageViewModel);
    }
}
