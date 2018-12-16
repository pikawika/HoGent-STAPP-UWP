using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Base;
using uwp_app_aalst_groep_a3.Models;
using uwp_app_aalst_groep_a3.Models.Domain;
using uwp_app_aalst_groep_a3.Network;
using uwp_app_aalst_groep_a3.Utils;
using Windows.UI.Notifications;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private ObservableCollection<Promotion> _promotions;

        public ObservableCollection<Promotion> Promotions
        {
            get { return _promotions; }
            set { _promotions = value; RaisePropertyChanged(nameof(Promotions));  }
        }

        private ObservableCollection<Establishment> _establishments;
        private MainPageViewModel mainPageViewModel;

        public ObservableCollection<Establishment> Establishments
        {
            get { return _establishments; }
            set { _establishments = value; RaisePropertyChanged(nameof(Establishments)); SubscriptionToastAsync(); }
        }

        private NetworkAPI NetworkAPI { get; set; }

        public RelayCommand EstablishmentClickedCommand { get; set; }

        public HomePageViewModel(MainPageViewModel mainPageViewModel)
        {
            this.mainPageViewModel = mainPageViewModel;
            EstablishmentClickedCommand = new RelayCommand((object args) => EstablishmentClicked(args));
            NetworkAPI = new NetworkAPI();
            InitializeHomePage();

            //SubscriptionToastAsync();
        }

        private async void SubscriptionToastAsync()
        {
            User user = await NetworkAPI.GetUser();
            if (user.UserId != -2)
            {
                bool changed = false;
                //eerst initialiseren anders null indien file niet bestaat
                List<Establishment> subs = new List<Establishment>();
                subs = await NetworkAPI.GetSubscriptions();

                //List<Establishment> merc = await NetworkAPI.GetSubscribedEstablishmentsAsync();

                bool isEqual = await NetworkAPI.CheckSubbedDifferenceByJSONAsync(subs);
                if (!isEqual)
                {
                    //als veranderd, dan toast tonen en wegschrijven van nieue subs
                    ToastNotificationManager.CreateToastNotifier().Show(new Toast().createToast("STAPP", "Er zijn nieuwe promoties of evenementen toegevoegd, bekijk ze hier!"));
                    await NetworkAPI.SaveSubscribedEstablishemtsAsync(subs);
                }
            }
            else
            {
                ToastNotificationManager.CreateToastNotifier().Show(new Toast().createToast("Welkom!", "Om alle functionaliteit van deze app te ontgrendelen, kan je hier aanmelden!"));
            }
            
        }

        private async void InitializeHomePage()
        {
            Promotions = new ObservableCollection<Promotion>(await NetworkAPI.GetAllPromotions());
            Establishments = new ObservableCollection<Establishment>(await NetworkAPI.GetAllEstablishments());
        }

        private void EstablishmentClicked(object args) => mainPageViewModel.NavigateTo(new EstablishmentDetailViewModel(args as Establishment, mainPageViewModel));

    }
}
