using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Utils;
using uwp_app_aalst_groep_a3.Views;
using Windows.UI.Xaml.Controls;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private ViewModelBase _currentData;

        public ViewModelBase CurrentData
        {
            get { return _currentData; }
            set { _currentData = value; RaisePropertyChanged(); }
        }

        public RelayCommand NavigationCommand { get; set; }

        private ObservableCollection<NavigationViewItem> _navigationViewItems;

        public ObservableCollection<NavigationViewItem> NavigationViewItems
        {
            get { return _navigationViewItems; }
            set { _navigationViewItems = value; RaisePropertyChanged(nameof(NavigationViewItems)); }
        }

        public NavigationViewItem SelectedItem { get; set; }

        public MainPageViewModel()
        {
            CurrentData = new HomePageViewModel();

            NavigationCommand = new RelayCommand((object args) => Navigate(args));

            NavigationViewItems = new ObservableCollection<NavigationViewItem>(CreateNavigationViewItems());

            SelectedItem = NavigationViewItems.FirstOrDefault();
        }

        private List<NavigationViewItem> CreateNavigationViewItems()
        {
            List<NavigationViewItem> items = new List<NavigationViewItem>();

            items.Add(new NavigationViewItem() { Icon = new SymbolIcon(Symbol.Home), Content = "Home", Tag = "Home" });
            items.Add(new NavigationViewItem() { Icon = new SymbolIcon(Symbol.Map), Content = "Kaart", Tag = "Map" });
            items.Add(new NavigationViewItem() { Icon = new SymbolIcon(Symbol.People), Content = "Handelaars", Tag = "Merchants" });
            items.Add(new NavigationViewItem() { Icon = new SymbolIcon(Symbol.OutlineStar), Content = "Evenementen", Tag = "Events" });

            return items;
        }

        private void Navigate(object args)
        {
            if (((NavigationViewItemInvokedEventArgs)args).IsSettingsInvoked)
            {
                CurrentData = new SettingsViewModel();
            }
            else
            {
                string selected = ((NavigationViewItemInvokedEventArgs)args).InvokedItem.ToString();

                switch (selected)
                {
                    case "Home":
                        CurrentData = new HomePageViewModel();
                        break;
                    case "Kaart":
                        CurrentData = new MapViewModel() { CurrentData = CurrentData};
                        break;
                    case "Handelaars":
                        CurrentData = new MerchantsViewModel();
                        break;
                    case "Evenementen":
                        CurrentData = new EventsViewModel();
                        break;
                    default:
                        CurrentData = new HomePageViewModel();
                        break;
                }
            }
        }
    }
}
