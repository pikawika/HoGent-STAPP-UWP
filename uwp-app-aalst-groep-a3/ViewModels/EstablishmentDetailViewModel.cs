using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Base;
using uwp_app_aalst_groep_a3.Models;
using uwp_app_aalst_groep_a3.Models.Domain;
using uwp_app_aalst_groep_a3.Network;
using uwp_app_aalst_groep_a3.Utils;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Security.Credentials;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class EstablishmentDetailViewModel : ViewModelBase
    {
        private readonly string is_subbed_text = "Niet meer volgen";
        private readonly string is_not_subbed_text = "Abonneren";

        public bool isSubscribed = false;
        private string _subscriptionButtonText = "Abonneren";

        public string SubscriptionButtonText
        {
            get { return _subscriptionButtonText; }
            set { _subscriptionButtonText = value; RaisePropertyChanged(nameof(SubscriptionButtonText)); }
        }

        private NetworkAPI networkAPI = new NetworkAPI();
        private PasswordVault passwordVault = new PasswordVault();

        public BasicGeoposition EstablishmentPosition { get; set; }
        public Geopoint EstablishmentPoint { get; set; }

        public List<MapIcon> MapIcons { get; set; }

        private ObservableCollection<MapLayer> _merchantMarkers;

        public ObservableCollection<MapLayer> MerchantMarkers
        {
            get { return _merchantMarkers; }
            set { _merchantMarkers = value; RaisePropertyChanged(nameof(MerchantMarkers)); }
        }

        public Establishment _establishment { get; set; }
        public Establishment Establishment
        {
            get { return _establishment; }
            set { _establishment = value; RaisePropertyChanged(nameof(Establishment)); }
        }

        private MainPageViewModel mainPageViewModel;

        public RelayCommand PromotionClickedCommand { get; set; }
        public RelayCommand EventClickedCommand { get; set; }
        public RelayCommand OpeningsurenCommand { get; set; }
        public RelayCommand MapElementClickCommand { get; set; }
        public RelayCommand SubscribeCommand { get; set; }

        private Visibility _merchantVisibility = Visibility.Collapsed;
        public Visibility MerchantVisibility
        {
            get { return _merchantVisibility; }
            set { _merchantVisibility = value; RaisePropertyChanged(nameof(MerchantVisibility)); }
        }

        public RelayCommand EditEstablishmentCommand { get; set; }
        public RelayCommand DeleteEstablishmentCommand { get; set; }

        private Visibility _facebookVisibility = Visibility.Collapsed;
        public Visibility FacebookVisibility
        {
            get { return _facebookVisibility; }
            set { _facebookVisibility = value; RaisePropertyChanged(nameof(FacebookVisibility)); }
        }

        private Visibility _twitterVisibility = Visibility.Collapsed;
        public Visibility TwitterVisibility
        {
            get { return _twitterVisibility; }
            set { _twitterVisibility = value; RaisePropertyChanged(nameof(TwitterVisibility)); }
        }

        private Visibility _instagramVisibility = Visibility.Collapsed;
        public Visibility InstagramVisibility
        {
            get { return _instagramVisibility; }
            set { _instagramVisibility = value; RaisePropertyChanged(nameof(InstagramVisibility)); }
        }

        public RelayCommand SocialMediaClickedCommand { get; set; }

        public EstablishmentDetailViewModel(Establishment establishment, MainPageViewModel mainPageViewModel)
        {
            Establishment = establishment;
            this.mainPageViewModel = mainPageViewModel;

            HandleEmptyEvents();
            HandleEmptyPromotions();

            MakeSocialMediaVisible();

            PromotionClickedCommand = new RelayCommand((object args) => EstablishmentClicked(args));
            EventClickedCommand = new RelayCommand((object args) => EventClicked(args));
            OpeningsurenCommand = new RelayCommand((args) => ShowOpeningHoursAsync());
            MapElementClickCommand = new RelayCommand((object args) => MapElementClicked(args));
            SubscribeCommand = new RelayCommand(async _ => await Subscribe());

            EditEstablishmentCommand = new RelayCommand(_ => EditEstablishment());
            DeleteEstablishmentCommand = new RelayCommand(async _ => await DeleteEstablishmentDialog());

            SocialMediaClickedCommand = new RelayCommand((object args) => SocialMediaClickedAsync(args));

            CheckMerchantOwnsPromotion();

            SetupSubscriptionButtonAsync();
            initMap();
        }

        private void MakeSocialMediaVisible()
        {
            if (Establishment.EstablishmentSocialMedias.Exists(esm => esm.SocialMedia.Name.ToLower() == "facebook")) FacebookVisibility = Visibility.Visible;
            if (Establishment.EstablishmentSocialMedias.Exists(esm => esm.SocialMedia.Name.ToLower() == "twitter")) TwitterVisibility = Visibility.Visible;
            if (Establishment.EstablishmentSocialMedias.Exists(esm => esm.SocialMedia.Name.ToLower() == "instagram")) InstagramVisibility = Visibility.Visible;
        }

        private async void SocialMediaClickedAsync(object args)
        {
            var type = args as string;
            var link = "";
            switch(type)
            {
                case "Facebook":
                    link = Establishment.EstablishmentSocialMedias.SingleOrDefault(esm => esm.SocialMedia.Name.ToLower() == "facebook").Url;
                    break;
                case "Twitter":
                    link = Establishment.EstablishmentSocialMedias.SingleOrDefault(esm => esm.SocialMedia.Name.ToLower() == "twitter").Url;
                    break;
                case "Instagram":
                    link = Establishment.EstablishmentSocialMedias.SingleOrDefault(esm => esm.SocialMedia.Name.ToLower() == "instagram").Url;
                    break;
            }
            if (link != "") await OpenSocialMedia(link);
        }

        private async Task OpenSocialMedia(string link)
        {
            var uri = new Uri(link);
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        private async void SetupSubscriptionButtonAsync()
        {
            List<Establishment> establishments_subscribed = await networkAPI.GetSubscriptions();

            if (establishments_subscribed.Where(e => e.EstablishmentId == Establishment.EstablishmentId).ToList().Count != 0)
            {
                isSubscribed = true;
            }

            if (isSubscribed)
            {
                SubscriptionButtonText = is_subbed_text;
            }
            else
            {
                SubscriptionButtonText = is_not_subbed_text;
            }
        }

        private void HandleEmptyEvents()
        {
            Models.Domain.Image image = new Models.Domain.Image() { Path = "img/establishments/none/empty.jpg" };
            List<Models.Domain.Image> images = new List<Models.Domain.Image>();
            images.Add(image);


            if (Establishment.Events.Count == 0)
            {

                Event e = new Event()
                {
                    Name = "Er zijn nog geen events toegevoegd",
                    Images = images
                };
                Establishment.Events.Add(e);
            }
        }

        private void HandleEmptyPromotions()
        {
            Models.Domain.Image image = new Models.Domain.Image() { Path = "img/establishments/none/empty.jpg" };
            List<Models.Domain.Image> images = new List<Models.Domain.Image>();
            images.Add(image);

            if (Establishment.Promotions.Count == 0)
            {
                Promotion p = new Promotion()
                {
                    Name = "Er zijn nog geen promoties toegevoegd",
                    Images = images
                };

                Establishment.Promotions.Add(p);
            }
        }

        private async void EstablishmentClicked(object args)
        {
            ContentDialog contentDialog = new ContentDialog();
            Promotion p = args as Promotion;

            p.Establishment = Establishment;

            string start = p.StartDate.ToString("d MMMM yyyy");
            string end = p.EndDate.ToString("d MMMM yyyy");

            contentDialog.Title = p.Name;

            if (p.Name != "Er zijn nog geen promoties toegevoegd")
            {
                contentDialog.Content = p.Message + "\n" + "Geldig van " + start + " tot " + end + ".";

                contentDialog.PrimaryButtonText = "Bekijk promotie";
                contentDialog.DefaultButton = ContentDialogButton.Primary;

                contentDialog.PrimaryButtonCommand = new RelayCommand((object o) => NavigateToPromotionDetail(o));
                contentDialog.PrimaryButtonCommandParameter = p;
            }

            contentDialog.CloseButtonText = "Sluiten";

            await contentDialog.ShowAsync();
        }

        private async void EventClicked(object args)
        {
            ContentDialog contentDialog = new ContentDialog();
            Event e = args as Event;

            e.Establishment = Establishment;

            string start = e.StartDate.ToString("d MMMM yyyy");
            string end = e.EndDate.ToString("d MMMM yyyy");

            if (e.Name != "Er zijn nog geen events toegevoegd")
            {
                contentDialog.Content = e.Message + "\n" + "Geldig van " + start + " tot " + end;

                contentDialog.PrimaryButtonText = "Bekijk evenement";
                contentDialog.DefaultButton = ContentDialogButton.Primary;

                contentDialog.PrimaryButtonCommand = new RelayCommand((object o) => NavigateToEventDetail(o));
                contentDialog.PrimaryButtonCommandParameter = e;
            }

            contentDialog.Title = e.Name;
            contentDialog.CloseButtonText = "Sluiten";

            await contentDialog.ShowAsync();
        }

        //kaart
        private void initMap()
        {
            EstablishmentPosition = new BasicGeoposition()
            {
                Latitude = Establishment.Latitude,
                Longitude = Establishment.Longitude,
            };

            EstablishmentPoint = new Geopoint(EstablishmentPosition);

            MerchantMarkers = new ObservableCollection<MapLayer>();

            RetrieveMerchantLocations();
        }

        private void RetrieveMerchantLocations()
        {
            MapIcons = new List<MapIcon>();

            MapIcons.Add(CreateMerchantMarker(Establishment));

            var landmarkLayer = new MapElementsLayer
            {
                ZIndex = 0,
                MapElements = new List<MapElement>(MapIcons)
            };

            MerchantMarkers.Add(landmarkLayer);
        }

        private MapIcon CreateMerchantMarker(Establishment e)
        {
            BasicGeoposition bg = new BasicGeoposition() { Latitude = e.Latitude, Longitude = e.Longitude };
            Geopoint gp = new Geopoint(bg);

            MapIcon mapIcon = new MapIcon();
            mapIcon.Location = gp;
            mapIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon.Title = e.Name;
            mapIcon.ZIndex = 0;

            return mapIcon;
        }

        private void MapElementClicked(object args)
        {
            string selected = (((MapElementClickEventArgs)args).MapElements.First() as MapIcon).Title;
            ShowAdressDialogAsync();
        }

        private async void ShowAdressDialogAsync()
        {
            ContentDialog contentDialog = new ContentDialog();

            contentDialog.Title = Establishment.Name;
            contentDialog.Content = $"{Establishment.Street} {Establishment.HouseNumber}\n{Establishment.PostalCode} {Establishment.City}";
            contentDialog.CloseButtonText = "Sluiten";

            contentDialog.PrimaryButtonText = "Bekijk kaart";
            contentDialog.DefaultButton = ContentDialogButton.Primary;

            contentDialog.PrimaryButtonCommand = new RelayCommand(_ => NavigateToMap());

            await contentDialog.ShowAsync();
        }

        private async void ShowOpeningHoursAsync()
        {
            ContentDialog contentDialog = new ContentDialog();

            string[] dagNamen = { "Maandag", "Dinsdag", "Woensdag", "Donderdag", "Vrijdag", "Zaterdag", "Zondag" };

            contentDialog.Title = "Openingsuren";

            string days = "";

            foreach (OpenDay day in Establishment.OpenDays)
            {
                days += "\n" + dagNamen[day.DayOfTheWeek] + ":\n";
                if (day.OpenHours.Count == 0)
                {
                    days += "Gesloten\n";
                }
                foreach (OpenHour hour in day.OpenHours)
                {
                    //int is soms = 0, moet dan 00 worden
                    string startMinute = hour.Startminute.ToString();
                    if (startMinute.Length == 1)
                    {
                        startMinute += "0";
                    }

                    //int is soms = 0, moet dan 00 worden
                    string endMinute = hour.Startminute.ToString();
                    if (endMinute.Length == 1)
                    {
                        endMinute += "0";
                    }
                    days += hour.StartHour + ":" + startMinute + " - " + hour.EndHour + ":" + endMinute + "\n";
                }
            }

            if (Establishment.ExceptionalDays.Count != 0)
            {
                days += "\nUitzonderlijk gesloten: \n";
                foreach (ExceptionalDay exceptionalDay in Establishment.ExceptionalDays)
                {
                    days += exceptionalDay.Day.ToString("d MMMM yyyy") + ": " + exceptionalDay.Message + "\n";
                }
            }

            contentDialog.Content = days;
            contentDialog.CloseButtonText = "Sluiten";

            await contentDialog.ShowAsync();
        }

        private async Task ShowNotSignedInDialog(string title, string message)
        {
            ContentDialog contentDialog = new ContentDialog();

            contentDialog.Title = title;
            contentDialog.Content = message;
            contentDialog.PrimaryButtonText = "Naar accountpagina";
            contentDialog.DefaultButton = ContentDialogButton.Primary;
            contentDialog.PrimaryButtonCommand = new RelayCommand(_ => NavigateToLogin());
            contentDialog.SecondaryButtonText = "Annuleren";

            await contentDialog.ShowAsync();
        }

        private async Task Subscribe()
        {
            if (isSubscribed)
            {
                var message = await networkAPI.Unsubscribe(Establishment.EstablishmentId);
                if (string.IsNullOrEmpty(message))
                {
                    SubscriptionButtonText = is_not_subbed_text;
                    isSubscribed = false;
                    await MessageUtils.ShowDialog("Abonneren", $"U zal geen meldingen meer ontvangen van {Establishment.Name}!");
                    Toast toast = new Toast();
                    toast.SubscriptionAsyncWriteOnly();
                }
                else
                {
                    await MessageUtils.ShowDialog("Abonneren", message);
                }
            }
            else
            {
                try
                {
                    var token = passwordVault.Retrieve("Stapp", "Token");
                    var message = await networkAPI.Subscribe(Establishment.EstablishmentId);
                    if (string.IsNullOrEmpty(message))
                    {
                        isSubscribed = true;
                        SubscriptionButtonText = is_subbed_text;
                        await MessageUtils.ShowDialog("Abonneren", $"U bent succesvol geabonneerd op {Establishment.Name}!");
                        mainPageViewModel.BackButtonPressed();
                        mainPageViewModel.NavigationHistoryItems.RemoveAll(v => v.GetType() == typeof(EstablishmentDetailViewModel));
                        mainPageViewModel.NavigateTo(new EstablishmentDetailViewModel(Establishment, mainPageViewModel));
                    }
                    else
                    {
                        await MessageUtils.ShowDialog("Abonneren", message);
                    }
                }
                catch
                {
                    await ShowNotSignedInDialog("Abonneren", "U bent momenteel niet aangemeld. Om te kunnen abonneren op handelaars, heeft u een account nodig. Aanmelden of een account aanmaken kan u doen op de accountpagina.");
                }
            }
        }

        private async void CheckMerchantOwnsPromotion()
        {
            try
            {
                var role = UserUtils.GetUserRole();
                if (role.ToLower() == "merchant")
                {
                    bool isOwner = await networkAPI.IsOwnerOfEstablishment(Establishment.EstablishmentId);

                    if (isOwner) MerchantVisibility = Visibility.Visible;
                }
            }
            catch { }
        }

        private void EditEstablishment() => mainPageViewModel.NavigateTo(new MerchantEditViewModel(MerchantObjectType.ESTABLISHMENT, mainPageViewModel, null, Establishment));

        private async Task DeleteEstablishmentDialog()
        {
            ContentDialog contentDialog = new ContentDialog();

            contentDialog.Title = "Vestiging verwijderen";
            contentDialog.Content = "Bent u zeker dat u deze vestiging wilt verwijderen?";
            contentDialog.PrimaryButtonText = "Ja";
            contentDialog.CloseButtonText = "Nee";

            contentDialog.PrimaryButtonCommand = new RelayCommand(async _ => await DeleteEstablishment());

            await contentDialog.ShowAsync();
        }

        private async Task DeleteEstablishment()
        {
            var message = await networkAPI.DeleteEstablishment(Establishment.EstablishmentId);
            await MessageUtils.ShowDialog("Vestiging verwijderen", message.Item1);
            if (message.Item2)
            {
                mainPageViewModel.BackButtonPressed();
                mainPageViewModel.NavigationHistoryItems.RemoveAll(v => v.GetType() == typeof(EstablishmentDetailViewModel));
            }
        }

        private void NavigateToLogin() => mainPageViewModel.NavigateTo(new LoginViewModel(mainPageViewModel));

        private void NavigateToPromotionDetail(object args) => mainPageViewModel.NavigateTo(new PromotionDetailViewModel(args as Promotion, mainPageViewModel));

        private void NavigateToEventDetail(object args) => mainPageViewModel.NavigateTo(new EventDetailViewModel(args as Event, mainPageViewModel));

        private void NavigateToMap() => mainPageViewModel.NavigateTo(new MapViewModel(mainPageViewModel));

    }

}
