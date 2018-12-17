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

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class SubscriptionsViewModel : ViewModelBase
    {
        private bool _loading = true;

        public bool Loading
        {
            get { return _loading; }
            set { _loading = value; RaisePropertyChanged(nameof(Loading)); Shown = value; }
        }

        public bool Shown
        {
            get { return !_loading; }
            set { _loading = value; RaisePropertyChanged(nameof(Shown)); }
        }

        private MainPageViewModel mainPageViewModel;

        private NetworkAPI NetworkAPI = new NetworkAPI();

        private ObservableCollection<Establishment> _subscriptions;

        public ObservableCollection<Establishment> Subscriptions
        {
            get { return _subscriptions; }
            set { _subscriptions = value; RaisePropertyChanged(nameof(Subscriptions)); Loading = false; }
        }

        private ObservableCollection<Promotion> _promotions = new ObservableCollection<Promotion>();

        public ObservableCollection<Promotion> Promotions
        {
            get { return _promotions; }
            set { _promotions = value; RaisePropertyChanged(nameof(Promotions)); }
        }

        private ObservableCollection<Event> _events = new ObservableCollection<Event>();

        public ObservableCollection<Event> Events
        {
            get { return _events; }
            set { _events = value; RaisePropertyChanged(nameof(Events)); HandleEmpty(); }
        }

        public RelayCommand SubscriptionClickedCommand { get; set; }

        public RelayCommand PromotionClickedCommand { get; set; }

        public RelayCommand EventClickedCommand { get; set; }

        public SubscriptionsViewModel(MainPageViewModel mainPageViewModel)
        {
            this.mainPageViewModel = mainPageViewModel;

            SubscriptionClickedCommand = new RelayCommand((object args) => SubscriptionClicked(args));

            PromotionClickedCommand = new RelayCommand((object args) => PromotionClicked(args));

            EventClickedCommand = new RelayCommand((object args) => EventClicked(args));

            InitializeHomePage();
            //HandleEmpty();
        }

        private void HandleEmpty()
        {
            Models.Domain.Image image = new Models.Domain.Image() { Path = "img/establishments/none/empty.jpg" };
            List<Models.Domain.Image> images = new List<Models.Domain.Image>();
            images.Add(image);

            if(Subscriptions.Count == 0)
            {
                Establishment establishment = new Establishment()
                {
                    Name = "Je hebt nog geen abonnementen",
                    Images = images
                };
                Subscriptions.Add(establishment);
            }

            if (Promotions.Count == 0)
            {
                Promotion p = new Promotion()
                {
                    Name = "Er zijn nog geen promoties toegevoegd",
                    Images = images
                };

                Promotions.Add(p);
            }

            if (Events.Count == 0)
            {
                Event e = new Event()
                {
                    Name = "Er zijn nog geen events toegevoegd",
                    Images = images
                };

                Events.Add(e);
            }
        }

        private async void InitializeHomePage()
        {
            Subscriptions = new ObservableCollection<Establishment>(await NetworkAPI.GetSubscriptions());

            var promotionList = new List<Promotion>();
            var eventList = new List<Event>();

            foreach (Establishment s in Subscriptions)
            {
                foreach (Promotion p in s.Promotions) {
                    p.Establishment = s;
                    promotionList.Add(p);
                }

                foreach (Event e in s.Events) {
                    e.Establishment = s;
                    eventList.Add(e);
                }
            }

            Promotions = new ObservableCollection<Promotion>(promotionList);
            Events = new ObservableCollection<Event>(eventList);

            
        }

        private void SubscriptionClicked(object args) => mainPageViewModel.NavigateTo(new EstablishmentDetailViewModel(args as Establishment, mainPageViewModel));

        private void PromotionClicked(object args) => mainPageViewModel.NavigateTo(new PromotionDetailViewModel(args as Promotion, mainPageViewModel));

        private void EventClicked(object args) => mainPageViewModel.NavigateTo(new EventDetailViewModel(args as Event, mainPageViewModel));
    }
}
