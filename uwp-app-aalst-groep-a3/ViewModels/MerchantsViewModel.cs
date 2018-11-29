using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Models;
using uwp_app_aalst_groep_a3.Network;
using uwp_app_aalst_groep_a3.Utils;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class MerchantsViewModel : ViewModelBase
    {

        private ObservableCollection<Establishment> _establishments;

        public ObservableCollection<Establishment> Establishments
        {
            get { return _establishments; }
            set { _establishments = value; RaisePropertyChanged(nameof(Establishments)); }
        }

        private NetworkAPI NetworkAPI { get; set; }

        public RelayCommand EstablishmentClickedCommand { get; set; }

        private MainPageViewModel mainPageViewModel;

        public MerchantsViewModel(MainPageViewModel mainPageViewModel)
        {
            this.mainPageViewModel = mainPageViewModel;
            EstablishmentClickedCommand = new RelayCommand((object args) => EstablishmentClicked(args));
            NetworkAPI = new NetworkAPI();
            InitializeHomePage();
        }

        private async void InitializeHomePage()
        {
            Establishments = new ObservableCollection<Establishment>(await NetworkAPI.GetAllEstablishments());
        }

        private void EstablishmentClicked(object args) => mainPageViewModel.CurrentData = new EstablishmentDetailViewModel(args as Establishment);
    }
}
