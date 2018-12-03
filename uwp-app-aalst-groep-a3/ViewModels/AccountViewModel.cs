using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private Customer _customer;

        public Customer Customer
        {
            get { return _customer; }
            set { _customer = value; RaisePropertyChanged(nameof(Customer)); }
        }


        public RelayCommand SignOutCommand { get; set; }

        public AccountViewModel(MainPageViewModel mainPageViewModel)
        {
            this.mainPageViewModel = mainPageViewModel;

            Customer = new Customer { FirstName = "", LastName = "", Email = "", Login = new Login() { Username = "" } };

            SignOutCommand = new RelayCommand(async _ => await SignOutAsync());

            GetCustomer();
        }

        private async void GetCustomer()
        {
            Customer = await networkAPI.GetCustomer();
        }

        private async Task SignOutAsync()
        {
            PasswordCredential pc = passwordVault.Retrieve("Stapp", "Token");
            passwordVault.Remove(pc);
            NavigateToLogin();
            await ShowDialog("Afmelden", "U bent succesvol afgemeld.");
        }

        private async Task ShowDialog(string title, string message)
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
