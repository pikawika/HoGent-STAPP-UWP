using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Base;
using uwp_app_aalst_groep_a3.Models;
using uwp_app_aalst_groep_a3.Network;
using uwp_app_aalst_groep_a3.Utils;
using Windows.UI.Xaml.Controls;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class MerchantsViewModel : ViewModelBase
    {
        private MainPageViewModel mainPageViewModel;
        private NetworkAPI NetworkAPI = new NetworkAPI();


        private ObservableCollection<Establishment> _establishments;

        //vars voor zoeken
        private ObservableCollection<string> _establishment_names;
        private List<Establishment> _all_establishments { get; set; }
        private string _SearchText = "";

        //properties met propertychanged
        public ObservableCollection<string> Establishment_Names {
            get { return _establishment_names; }
            set { _establishment_names = value; RaisePropertyChanged(nameof(Establishment_Names)); }
        }
        
        public string SearchText {
            get { return _SearchText; }
            set { _SearchText = value; RaisePropertyChanged(nameof(SearchText)); }
        }
    
        public ObservableCollection<Establishment> Establishments
        {
            get { return _establishments; }
            set { _establishments = value; RaisePropertyChanged(nameof(Establishments)); }
        }

        //commands
        public RelayCommand EstablishmentClickedCommand { get; set; }
        public RelayCommand TextChangedCommand { get; set; }

        public MerchantsViewModel(MainPageViewModel mainPageViewModel)
        {
            this.mainPageViewModel = mainPageViewModel;
            _all_establishments = new List<Establishment>();
            _establishment_names = new ObservableCollection<string>();
            EstablishmentClickedCommand = new RelayCommand((args) => EstablishmentClicked(args));
            TextChangedCommand = new RelayCommand((args) => Search(args));
            InitializeHomePage();

        }


        private void Search(object args)
        {
            Debug.WriteLine("Mijn searchtext:" + _SearchText);
            Establishment_Names = new ObservableCollection<string>(_all_establishments.Where(e => e.Name.IndexOf(_SearchText, StringComparison.OrdinalIgnoreCase) >= 0).Select(e => e.Name).ToList());
            Establishments = new ObservableCollection<Establishment>(_all_establishments.Where(e => e.Name.IndexOf(_SearchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList());
        }



        private async void InitializeHomePage() {
            Establishments = new ObservableCollection<Establishment>(await NetworkAPI.GetAllEstablishments());
            _all_establishments = Establishments.ToList();
            Establishment_Names = new ObservableCollection<string>(Establishments.Select(e => e.Name).ToList());
        }

        private void EstablishmentClicked(object args) => mainPageViewModel.CurrentData = new EstablishmentDetailViewModel(args as Establishment, mainPageViewModel);
    }
}
