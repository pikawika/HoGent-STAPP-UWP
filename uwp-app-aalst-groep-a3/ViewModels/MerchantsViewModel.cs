using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Models;
using uwp_app_aalst_groep_a3.Network;

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

        public MerchantsViewModel()
        {
            NetworkAPI = new NetworkAPI();
            InitializeHomePage();
        }

        private async void InitializeHomePage()
        {
            Establishments = new ObservableCollection<Establishment>(await NetworkAPI.GetAllEstablishments());
        }
    }
}
