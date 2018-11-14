using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Models;
using uwp_app_aalst_groep_a3.Models.Domain;
using uwp_app_aalst_groep_a3.Network;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private ObservableCollection<Promotion> _promotions;

        public ObservableCollection<Promotion> Promotions
        {
            get { return _promotions; }
            set { _promotions = value; RaisePropertyChanged(nameof(Promotions)); }
        }

        private ObservableCollection<Establishment> _establishments;

        public ObservableCollection<Establishment> Establishments
        {
            get { return _establishments; }
            set { _establishments = value; RaisePropertyChanged(nameof(Establishments)); }
        }

        private NetworkAPI NetworkAPI { get; set; }

        public HomePageViewModel()
        {
            NetworkAPI = new NetworkAPI();
            InitializeHomePage();
        }

        private async void InitializeHomePage()
        {
            Promotions = await NetworkAPI.GetAllPromotions();
            Establishments = await NetworkAPI.GetAllEstablishments();
        }

    }
}
