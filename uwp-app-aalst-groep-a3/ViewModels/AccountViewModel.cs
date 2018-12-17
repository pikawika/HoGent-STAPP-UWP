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

        public AccountViewModel(MainPageViewModel mainPageViewModel)
        {
            this.mainPageViewModel = mainPageViewModel;

            User = new User { FirstName = "", LastName = "", Email = "", Login = new Login() { Username = "" } };

            SignOutCommand = new RelayCommand(async _ => await SignOutAsync());

            ShowSubscriptionsCommand = new RelayCommand(_ => ShowPageMatchingRole());

            GetUser();

            var role = UserUtils.GetUserRole();

            if (role.ToLower() == "customer") ButtonText = "Bekijk abonnementen";
            else if (role.ToLower() == "merchant") ButtonText = "Bekijk uw overzicht";
        }

        private async void GetUser()
        {
            User = await networkAPI.GetUser();
        }

        private async Task SignOutAsync()
        {
            NavigateToLogin();
            mainPageViewModel.NavigationHistoryItems.RemoveAll(v =>
                v.GetType() == typeof(AccountViewModel) ||
                v.GetType() == typeof(SubscriptionsViewModel) ||
                v.GetType() == typeof(MerchantPanelViewModel) ||
                v.GetType() == typeof(MerchantAddViewModel));

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
