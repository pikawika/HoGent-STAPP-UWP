using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Utils;
using Windows.UI.Xaml.Controls;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public Frame MainFrame { get; set; }
        public RelayCommand ShowEventsCommand { get; set; }

        private ViewModelBase _currentData;

        public ViewModelBase CurrentData
        {
            get { return _currentData; }
            set { _currentData = value; RaisePropertyChanged(); }
        }


        public MainPageViewModel()
        {
            ShowEventsCommand = new RelayCommand(_ => ShowEvents());
        }

        private void ShowEvents()
        {
            //CurrentData = new MoviesViewModel();
        }
    }
}
