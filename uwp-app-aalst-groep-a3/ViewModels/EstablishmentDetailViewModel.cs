using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Models;
using uwp_app_aalst_groep_a3.Models.Domain;
using uwp_app_aalst_groep_a3.Network;
using uwp_app_aalst_groep_a3.Utils;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class EstablishmentDetailViewModel : ViewModelBase
    {
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
        public RelayCommand MapElementClickCommand { get; set; }
        public RelayCommand SubscribeCommand { get; set; }

        public EstablishmentDetailViewModel(Establishment establishment, MainPageViewModel mainPageViewModel)
        {
            Establishment = establishment;
            this.mainPageViewModel = mainPageViewModel;

            HandleEmptyEvents();
            HandleEmptyPromotions();

            PromotionClickedCommand = new RelayCommand((object args) => EstablishmentClicked(args));
            EventClickedCommand = new RelayCommand((object args) => EventClicked(args));
            MapElementClickCommand = new RelayCommand((object args) => MapElementClicked(args));
            SubscribeCommand = new RelayCommand(async _ => await Subscribe());

            initMap();
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

            DateTime startDt = DateTime.ParseExact(p.StartDate.ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            string start = startDt.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);

            DateTime endDt = DateTime.ParseExact(p.EndDate.ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            string end = endDt.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);

            contentDialog.Title = p.Name;
            contentDialog.Content = p.Message + "\n" + "Geldig van "+ start + " tot " + end;
            contentDialog.CloseButtonText = "Sluiten";

            await contentDialog.ShowAsync();
        }

        private async void EventClicked(object args)
        {
            ContentDialog contentDialog = new ContentDialog();
            Event e = args as Event;

            /*DateTime startDt = DateTime.ParseExact(e.StartDate.ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            string start = startDt.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);

            DateTime endDt = DateTime.ParseExact(e.EndDate.ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            string end = endDt.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
            */
            contentDialog.Title = e.Name;
            contentDialog.Content = e.Message /*+ "\n" + "Geldig van " + e.StartDate + " tot " + e.EndDate*/;
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
            contentDialog.Content = Establishment.Street + " " + Establishment.HouseNumber+ "," + Establishment.PostalCode + " " + Establishment.City;
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

        private async Task ShowDialog(string title, string message)
        {
            ContentDialog contentDialog = new ContentDialog();

            contentDialog.Title = title;
            contentDialog.Content = message;
            contentDialog.PrimaryButtonText = "Oké";

            await contentDialog.ShowAsync();
        }

        private void NavigateToLogin() => mainPageViewModel.CurrentData = new LoginViewModel(mainPageViewModel);

        private async Task Subscribe()
        {
            try
            {
                var token = passwordVault.Retrieve("Stapp", "Token");
                var message = await networkAPI.Subscribe(Establishment.EstablishmentId);
                if (string.IsNullOrEmpty(message))
                {
                    await ShowDialog("Abonneren", $"U bent succesvol geabonneerd op {Establishment.Name}!");
                }
                else
                {
                    await ShowDialog("Abonneren", $"Er is een fout opgetreden: {message}");
                }
            }
            catch
            {
                await ShowNotSignedInDialog("Abonneren", "U bent momenteel niet aangemeld. Om te kunnen abonneren op handelaars, heeft u een account nodig. Aanmelden of een account aanmaken kan u doen op de accountpagina.");
            }
        }
    }

}
