using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Models;
using uwp_app_aalst_groep_a3.Network;
using uwp_app_aalst_groep_a3.Utils;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class EventDetailViewModel : ViewModelBase
    {
        public Event Event { get; set; }
        private MainPageViewModel mainPageViewModel;
        public RelayCommand ShowEstbalishmentCommandClicked { get; set; }


        public EventDetailViewModel(Event Event, MainPageViewModel mainPageViewModel)
        {
            this.mainPageViewModel = mainPageViewModel;
            this.Event = Event;

            ShowEstbalishmentCommandClicked = new RelayCommand((object args) => ShowEstablishmentAsync());
        }

        private async Task ShowEstablishmentAsync() {
            NetworkAPI networkAPI = new NetworkAPI();
            Establishment establishment = await networkAPI.GetEstablishmentById(Event.Establishment.EstablishmentId);
            mainPageViewModel.CurrentData = new EstablishmentDetailViewModel(establishment);
        }
    }
}
