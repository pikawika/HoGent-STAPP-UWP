using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Utils;
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

        public MainPageViewModel()
        {
            NavigationCommand = new RelayCommand((object args) => Navigate(args));
        }

        private void Navigate(object args)
        {
            string selected = ((NavigationViewItemInvokedEventArgs)args).InvokedItem.ToString();
            switch (selected)
            {
                case "Home":
                    CurrentData = new HomePageViewModel();
                    break;
                case "Kaart":
                    CurrentData = new MapViewModel();
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
