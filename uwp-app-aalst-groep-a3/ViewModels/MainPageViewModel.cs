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

        public RelayCommand ShowHomeCommand { get; set; }
        public RelayCommand ShowMapCommand { get; set; }
        public RelayCommand ShowMerchantsCommand { get; set; }
        public RelayCommand ShowEventsCommand { get; set; }

        public MainPageViewModel()
        {
            ShowHomeCommand = new RelayCommand(_ => ShowHome());
            ShowMapCommand = new RelayCommand(_ => ShowMap());
            ShowMerchantsCommand = new RelayCommand(_ => ShowMerchants());
            ShowEventsCommand = new RelayCommand(_ => ShowEvents());
        }

        private void ShowHome()
        {
            Debug.WriteLine("Hallo");
            CurrentData = new HomePageViewModel();
        }

        private void ShowMap()
        {
            //CurrentData = new MoviesViewModel();
        }

        private void ShowMerchants()
        {
            //CurrentData = new MoviesViewModel();
        }

        private void ShowEvents()
        {
            //CurrentData = new MoviesViewModel();
        }
    }
}
