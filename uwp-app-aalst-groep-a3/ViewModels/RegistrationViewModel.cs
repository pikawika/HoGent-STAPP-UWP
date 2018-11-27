using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Utils;
using Windows.UI.Xaml.Controls;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class RegistrationViewModel : ViewModelBase
    {
        private MainPageViewModel mainPageViewModel;

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

            RegisterCommand = new RelayCommand(_ => CreateAccount());
            NavigateToLoginCommand = new RelayCommand(_ => NavigateToLogin());
        }

        private void CreateAccount()
        {
            if (string.IsNullOrWhiteSpace(FirstName)
                || string.IsNullOrWhiteSpace(LastName)
                || string.IsNullOrWhiteSpace(EmailAddress)
                || string.IsNullOrWhiteSpace(Username)
                || string.IsNullOrWhiteSpace(Password)
                || string.IsNullOrWhiteSpace(RepeatPassword))
            {
                ShowDialog("Aanmelding", "Gelieve in ieder veld een waarde in te voeren.");
                return;
            }

            if (!Regex.IsMatch(EmailAddress, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
            {
                ShowDialog("Aanmelding", "Gelieve een geldig e-mailadres in te voeren.");
                return;
            }

            if (Password != RepeatPassword)
            {
                ShowDialog("Aanmelding", "Wachtwoord en herhaal wachtwoord komen niet overeen.");
                return;
            }

            ShowDialog("Is 't goe?", "'t Is toch simpel hé!");
        }

        private async void ShowDialog(string title, string message)
        {
            ContentDialog contentDialog = new ContentDialog();

            contentDialog.Title = title;
            contentDialog.Content = message;
            contentDialog.PrimaryButtonText = "Oké";

            await contentDialog.ShowAsync();
        }

        private void NavigateToLogin() => mainPageViewModel.CurrentData = new LoginViewModel(mainPageViewModel);
    }
}
