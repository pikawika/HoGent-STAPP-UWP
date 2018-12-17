using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Base;
using uwp_app_aalst_groep_a3.Utils;
using uwp_app_aalst_groep_a3.Views;
using Windows.Security.Credentials;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        // De password vault wordt gebruikt om de token van een gebruiker in op te slaan
        private PasswordVault passwordVault = new PasswordVault();

        // De current data is steeds de huidige viewmodel, en adhv deze viewmodel wordt de bijhorende view getoond
        private ViewModelBase _currentData;
        public ViewModelBase CurrentData
        {
            get { return _currentData; }
            private set { _currentData = value; RaisePropertyChanged(); }
        }

        // De lijst van icoontjes die in de navigatiebalk zullen komen te staan
        private ObservableCollection<NavigationViewItem> _navigationViewItems;
        public ObservableCollection<NavigationViewItem> NavigationViewItems
        {
            get { return _navigationViewItems; }
            set { _navigationViewItems = value; RaisePropertyChanged(nameof(NavigationViewItems)); }
        }

        // De lijst van viewmodels die in de navigatie geschiedenis zitten
        public List<ViewModelBase> NavigationHistoryItems { get; set; } = new List<ViewModelBase>();

        // Het geselecteerd icoontje in de navigatiebalk
        // Wordt gebruikt voor het opstarten om aan te duiden dat de HomePageView de hoofdpagina is
        private NavigationViewItem _selectedItem { get; set; }
        public NavigationViewItem SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; RaisePropertyChanged(nameof(SelectedItem)); }
        }

        // Commando dat opgeroepen wordt zodra er op een icoontje in de navigatiebalk geklikt wordt
        public RelayCommand NavigationCommand { get; set; }

        public MainPageViewModel()
        {
            NavigationCommand = new RelayCommand((object args) => Navigate(args));

            NavigationViewItems = new ObservableCollection<NavigationViewItem>(CreateNavigationViewItems());

            // De navigatiebalk weet niet dat standaard het geselecteerde icoontje het huisje is, dus dit wordt manueel gedaan
            SelectedItem = NavigationViewItems.FirstOrDefault();

            // De homepage wordt meteen toegevoegd aan de navigatiegeschiedenis
            NavigationHistoryItems.Add(new HomePageViewModel(this));

            // Als de gebruiker aangemeld is, dan moet er een gepast extra icoontje in de navigatiebalk verschijnen
            try
            {
                var role = UserUtils.GetUserRole();
                if (role.ToLower() == "customer") AddSubscriptionNavigationViewItem();
                else if (role.ToLower() == "merchant") AddMerchantPanelNavigationViewItem();
            }
            catch
            {
                Debug.WriteLine("De gebruiker is niet aangemeld.");
            }

            CurrentData = NavigationHistoryItems[0];

            InitializeBackButton();
        }

        // Het aanmaken van alle icoontjes voor in de navigatiebalk
        // Instellingen staat hier niet bij omdat die standaard voorzien wordt
        private List<NavigationViewItem> CreateNavigationViewItems()
        {
            List<NavigationViewItem> items = new List<NavigationViewItem>();

            items.Add(new NavigationViewItem() { Icon = new SymbolIcon(Symbol.Home), Content = "Home", Tag = "Home" });
            items.Add(new NavigationViewItem() { Icon = new SymbolIcon(Symbol.Map), Content = "Kaart", Tag = "Map" });
            items.Add(new NavigationViewItem() { Icon = new SymbolIcon(Symbol.People), Content = "Handelaars", Tag = "Merchant" });
            items.Add(new NavigationViewItem() { Icon = new SymbolIcon(Symbol.Shop), Content = "Promoties", Tag = "Promotion" });
            items.Add(new NavigationViewItem() { Icon = new SymbolIcon(Symbol.OutlineStar), Content = "Evenementen", Tag = "Event" });
            items.Add(new NavigationViewItem() { Icon = new SymbolIcon(Symbol.Contact), Content = "Account", Tag = "Account" });

            return items;
        }

        // Toevoegen van het abonnement icoontje aan de navigatiebalk
        public void AddSubscriptionNavigationViewItem() => NavigationViewItems.Add(new NavigationViewItem() { Icon = new SymbolIcon(Symbol.Read), Content = "Abonnementen", Tag = "Subscription" });

        // Verwijderen van het abonnement icoontje in de navigatiebalk
        public void RemoveSubscriptionNavigationViewItem() => NavigationViewItems.Remove(NavigationViewItems.SingleOrDefault(n => n.Tag.ToString() == "Subscription"));

        // Toevoegen van het handelaar paneel icoontje aan de navigatiebalk
        public void AddMerchantPanelNavigationViewItem() => NavigationViewItems.Add(new NavigationViewItem() { Icon = new SymbolIcon(Symbol.PreviewLink), Content = "Mijn overzicht", Tag = "Panel" });

        // Verwijderen van het handelaar paneel icoontje in de navigatiebalk
        public void RemoveMerchantPanelNavigationViewItem() => NavigationViewItems.Remove(NavigationViewItems.SingleOrDefault(n => n.Tag.ToString() == "Panel"));

        // Methode voor het navigeren zodra er in de navigatiebalk op een icoontje geklikt wordt
        private void Navigate(object args)
        {
            if (((NavigationViewItemInvokedEventArgs)args).IsSettingsInvoked)
            {
                NavigateTo(new SettingsViewModel());
            }
            else
            {
                string selected = ((NavigationViewItemInvokedEventArgs)args).InvokedItem.ToString();

                switch (selected)
                {
                    case "Home":
                        NavigateTo(new HomePageViewModel(this));
                        break;
                    case "Kaart":
                        NavigateTo(new MapViewModel(this));
                        break;
                    case "Handelaars":
                        NavigateTo(new MerchantsViewModel(this));
                        break;
                    case "Promoties":
                        NavigateTo(new PromotionsViewModel(this));
                        break;
                    case "Evenementen":
                        NavigateTo(new EventsViewModel(this));
                        break;
                    case "Account":
                        // Als er een token in de password vault zit, is de gebruiker aangemeld
                        try
                        {
                            passwordVault.Retrieve("Stapp", "Token");
                            NavigateTo(new AccountViewModel(this));
                        }
                        // Zo niet, dan wordt de gebruiker doorgestuurd naar het login scherm
                        catch
                        {
                            NavigateTo(new LoginViewModel(this));
                        }
                        break;
                    case "Abonnementen":
                        NavigateTo(new SubscriptionsViewModel(this));
                        break;
                    case "Mijn overzicht":
                        NavigateTo(new MerchantPanelViewModel(this));
                        break;
                }
            }
        }

        // Ervoor zorgen dat er een terug knop beschikbaar is linksboven de app
        private void InitializeBackButton()
        {
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Disabled;
            currentView.BackRequested += OnBackRequested;
        }

        // Methode die wordt aangeroepen als de gebruiker op de terug knop klikt
        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (CanGoBack())
            {
                var index = NavigationHistoryItems.Count - 1;
                CurrentData = NavigationHistoryItems[index - 1];
                NavigationHistoryItems.RemoveAt(index);
                AdjustSelectedItem();
                AdjustBackButtonVisibility();
                e.Handled = true;
            }
        }

        // Checken of er meer dan één scherm zit in de navigatie geschiedenis
        private bool CanGoBack() { return NavigationHistoryItems.Count > 1; }

        // Navigeren naar een nieuwe viewmodel en toevoegen aan de navigatie geschiedenis
        public void NavigateTo(ViewModelBase viewModel)
        {
            if (CurrentData.GetType() != viewModel.GetType())
            {
                var index = NavigationHistoryItems.Count;
                NavigationHistoryItems.Add(viewModel);
                CurrentData = NavigationHistoryItems[index];
                AdjustSelectedItem();
                AdjustBackButtonVisibility();
            }
        }

        // Deze functie zorgt ervoor dat bij navigeren steeds het juiste incoontje geselecteerd is
        private void AdjustSelectedItem()
        {
            var current = CurrentData.GetType().ToString().ToLower();

            if (current.Contains("establishment")) current = "merchants";
            else if (current.Contains("login") || current.Contains("registration")) current = "account";
            else if (current.Contains("panel") || current.Contains("add")) current = "panel";

            SelectedItem = NavigationViewItems.SingleOrDefault(n => current.Contains(n.Tag.ToString().ToLower()));
        }
        
        // Deze functie zorgt ervoor dat de back button niet zichtbaar is als je niet meer terug kan gaan
        private void AdjustBackButtonVisibility()
        {
            var currentView = SystemNavigationManager.GetForCurrentView();
            if (NavigationHistoryItems.Count > 1) currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            else currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Disabled;
        }

    }
}
