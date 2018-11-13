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
        public ObservableCollection<Promotion> Promotions { get; set; }
        public ObservableCollection<Establishment> Establishments { get; set; }

        public HomePageViewModel()
        {
            InitializeHomePage();
        }

        private async void InitializeHomePage()
        {
            Promotions = await NetworkAPI.GetAllPromotions();
            Establishments = await NetworkAPI.GetAllEstablishments();
        }

    }
}
