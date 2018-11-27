using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Utils;
using Windows.UI.Xaml.Controls;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private MainPageViewModel mainPageViewModel;

        public string Gebruikersnaam { get; set; } = "";
        public string Wachtwoord { get; set; } = "";

        public RelayCommand SignInCommand { get; set; }
        public RelayCommand NavigateToRegistrationCommand { get; set; }

        public LoginViewModel(MainPageViewModel mainPageViewModel)
        {
            this.mainPageViewModel = mainPageViewModel;

            SignInCommand = new RelayCommand(_ => SignIn());
            NavigateToRegistrationCommand = new RelayCommand(_ => NavigateToRegistration());
        }

        private void SignIn()
        {
            if (Gebruikersnaam == "" || Wachtwoord == "")
            {
                ShowDialog("Aanmelding", "Gelieve zowel uw gebruikersnaam als uw wachtwoord in te voeren.");
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

        private void NavigateToRegistration() => mainPageViewModel.CurrentData = new RegistrationViewModel(mainPageViewModel);
    }
}
