using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
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
        }

        private void CreateMerchantMarkers()
        {

        }
    }
}
