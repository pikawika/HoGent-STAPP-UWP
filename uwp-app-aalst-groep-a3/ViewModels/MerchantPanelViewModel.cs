using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Base;
using uwp_app_aalst_groep_a3.Models;
using uwp_app_aalst_groep_a3.Network;
using uwp_app_aalst_groep_a3.Utils;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class MerchantPanelViewModel : ViewModelBase
    {
        private MainPageViewModel mainPageViewModel;

        private NetworkAPI NetworkAPI = new NetworkAPI();

        private ObservableCollection<Company> _companies;

        public ObservableCollection<Company> Companies
        {
            get { return _companies; }
            set { _companies = value; RaisePropertyChanged(nameof(Companies)); }
        }

        private ObservableCollection<Establishment> _establishments;

        public ObservableCollection<Establishment> Establishments
        {
            get { return _establishments; }
            set { _establishments = value; RaisePropertyChanged(nameof(Establishments)); }
        }

        private ObservableCollection<Promotion> _promotions;

        public ObservableCollection<Promotion> Promotions
        {
            get { return _promotions; }
            set { _promotions = value; RaisePropertyChanged(nameof(Promotions)); }
        }

        private ObservableCollection<Event> _events;

        public ObservableCollection<Event> Events
        {
            get { return _events; }
            set { _events = value; RaisePropertyChanged(nameof(Events)); }
        }

        public RelayCommand EstablishmentClickedCommand { get; set; }

        public RelayCommand PromotionClickedCommand { get; set; }

        public RelayCommand EventClickedCommand { get; set; }

        public MerchantPanelViewModel(MainPageViewModel mainPageViewModel)
        {
            this.mainPageViewModel = mainPageViewModel;

            EstablishmentClickedCommand = new RelayCommand((object args) => EstablishmentClicked(args));

            PromotionClickedCommand = new RelayCommand((object args) => PromotionClicked(args));

            EventClickedCommand = new RelayCommand((object args) => EventClicked(args));

            InitializeHomePage();
        }

        private async void InitializeHomePage()
        {
            Companies = new ObservableCollection<Company>(await NetworkAPI.GetCompanies());

            var establishmentList = new List<Establishment>();

            foreach (Company c in Companies)
            {
                foreach (Establishment e in c.Establishments)
                {
                    establishmentList.Add(e);
                }
            }

            Establishments = new ObservableCollection<Establishment>(establishmentList);

            var promotionList = new List<Promotion>();
            var eventList = new List<Event>();

            foreach (Establishment s in Establishments)
            {
                foreach (Promotion p in s.Promotions)
                {
                    p.Establishment = s;
                    promotionList.Add(p);
                }

                foreach (Event e in s.Events)
                {
                    e.Establishment = s;
                    eventList.Add(e);
                }
            }

            Promotions = new ObservableCollection<Promotion>(promotionList);
            Events = new ObservableCollection<Event>(eventList);
        }

        private void EstablishmentClicked(object args) => mainPageViewModel.NavigateTo(new EstablishmentDetailViewModel(args as Establishment, mainPageViewModel));

        private void PromotionClicked(object args) => mainPageViewModel.NavigateTo(new PromotionDetailViewModel(args as Promotion, mainPageViewModel));

        private void EventClicked(object args) => mainPageViewModel.NavigateTo(new EventDetailViewModel(args as Event, mainPageViewModel));
    }
}
