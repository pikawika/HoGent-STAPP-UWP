using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Models;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.UI.Xaml.Controls.Maps;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        public BasicGeoposition AalstPosition { get; }
        public Geopoint AalstPoint { get; }
        public Double MapZoomlevel { get; }

        public ObservableCollection<MapLayer> MerchantMarkers { get; }

        public MapViewModel()
        {
            AalstPosition = new BasicGeoposition()
            {
                Latitude = 50.937753,
                Longitude = 4.040881,
            };

            AalstPoint = new Geopoint(AalstPosition);

            MapZoomlevel = 14.5;

            MerchantMarkers = new ObservableCollection<MapLayer>();

            RetrieveMerchantLocations();
        }

        private void RetrieveMerchantLocations()
        {
            List<Establishment> establishments = DummyDataEstablishments();
            List<MapElement> mapIcons = new List<MapElement>();

            foreach (Establishment e in establishments)
            {
                mapIcons.Add(CreateMerchantMarker(e));
            }

            var landmarkLayer = new MapElementsLayer
            {
                ZIndex = 0,
                MapElements = mapIcons
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

        //Dit is maar een tijdelijke methode voor het ophalen van establishments
        private List<Establishment> DummyDataEstablishments()
        {
            List<Establishment> establishments = new List<Establishment>();
            Establishment e1 = new Establishment() { Name = "Fnac", Latitude = 50.939370, Longitude = 4.038105 };
            Establishment e2 = new Establishment() { Name = "Thuiszorgwinkel", Latitude = 50.937490, Longitude = 4.037666 };
            Establishment e3 = new Establishment() { Name = "Lab9", Latitude = 50.939608, Longitude = 4.037593 };
            Establishment e4 = new Establishment() { Name = "Comar Sport", Latitude = 50.940590, Longitude = 4.030851 };
            Establishment e5 = new Establishment() { Name = "De Banier", Latitude = 50.939765, Longitude = 4.042684 };
            establishments.Add(e1);
            establishments.Add(e2);
            establishments.Add(e3);
            establishments.Add(e4);
            establishments.Add(e5);
            return establishments;
        }
    }
}
