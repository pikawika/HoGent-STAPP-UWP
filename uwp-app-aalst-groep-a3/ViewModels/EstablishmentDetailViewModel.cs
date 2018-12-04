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
using uwp_app_aalst_groep_a3.Utils;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class EstablishmentDetailViewModel : ViewModelBase
    {
        public BasicGeoposition EstablishmentPosition { get; set; }
        public Geopoint EstablishmentPoint { get; set; }

        public List<MapIcon> MapIcons { get; set; }

        private ObservableCollection<MapLayer> _merchantMarkers;

        public RelayCommand MapElementClickCommand { get; set; }

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

        public RelayCommand PromotionClickedCommand { get; set; }
        public RelayCommand EventClickedCommand { get; set; }

        public EstablishmentDetailViewModel(Establishment establishment)
        {


            Establishment = establishment;

            handleEmptyEvents();
            handleEmptyPromotions();

            
            Establishment.Description = "Maecenas imperdiet tempor nisi ut rutrum. Donec sollicitudin tortor pharetra nunc lacinia posuere. Sed nisi odio, gravida sed enim non, porta elementum mauris. Ut id est sed nunc semper sagittis non sit amet felis.";

            PromotionClickedCommand = new RelayCommand((object args) => EstablishmentClicked(args));
            EventClickedCommand = new RelayCommand((object args) => EventClicked(args));

            initMap();
        }

        private void handleEmptyEvents()
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

        private void handleEmptyPromotions()
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

            Debug.WriteLine("test lol hihi: " + p.Name);

            string start = p.StartDate.ToString("d MMMM yyyy");
            string end = p.EndDate.ToString("d MMMM yyyy");

            contentDialog.Title = p.Name;

            if (p.Name != "Er zijn nog geen promoties toegevoegd")
            {
                contentDialog.Content = p.Message + "\n" + "Geldig van " + start + " tot " + end;
            }


            contentDialog.CloseButtonText = "Sluiten";

            await contentDialog.ShowAsync();
        }

        private async void EventClicked(object args)
        {
            ContentDialog contentDialog = new ContentDialog();
            Event e = args as Event;

            string start = e.StartDate.ToString("d MMMM yyyy");
            string end = e.EndDate.ToString("d MMMM yyyy");

            if (e.Name != "Er zijn nog geen events toegevoegd")
            {
                contentDialog.Content = e.Message + "\n" + "Geldig van " + start + " tot " + end;
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

            MapElementClickCommand = new RelayCommand((object args) => MapElementClicked(args));

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
    }

}
