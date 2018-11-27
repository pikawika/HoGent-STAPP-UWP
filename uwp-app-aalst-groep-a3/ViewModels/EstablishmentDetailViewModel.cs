using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Models;
using uwp_app_aalst_groep_a3.Models.Domain;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml.Controls.Maps;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class EstablishmentDetailViewModel : ViewModelBase
    {
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

        public EstablishmentDetailViewModel(Establishment establishment)
        {
            Image image = new Image() { Path = "img/establishments/none/empty.jpg" };
            List<Image> images = new List<Image>();
            images.Add(image);

            Establishment = establishment;
            if(Establishment.Events.Count == 0)
            {
                
                Event e = new Event()
                {
                    Name = "Er zijn nog geen events toegevoegd",
                    Images = images
                };
                Establishment.Events.Add(e);
            }

            if(Establishment.Promotions.Count == 0)
            {
                Promotion p = new Promotion()
                {
                    Name = "Er zijn nog geen promoties toegevoegd",
                    Images = images
                };

                Establishment.Promotions.Add(p);
            }
            Establishment.Description = "Maecenas imperdiet tempor nisi ut rutrum. Donec sollicitudin tortor pharetra nunc lacinia posuere. Sed nisi odio, gravida sed enim non, porta elementum mauris. Ut id est sed nunc semper sagittis non sit amet felis.";
            initMap();
        }


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

        private async void RetrieveMerchantLocations()
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
    }

}
