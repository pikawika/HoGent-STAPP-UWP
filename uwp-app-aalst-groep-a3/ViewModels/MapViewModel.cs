using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Models;
using uwp_app_aalst_groep_a3.Network;
using uwp_app_aalst_groep_a3.Utils;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        public BasicGeoposition AalstPosition { get; }
        public Geopoint AalstPoint { get; }
        public double MapZoomlevel { get; }
        public List<Establishment> Establishments { get; set; }
        public List<MapIcon> MapIcons { get; set; }

        private ObservableCollection<MapLayer> _merchantMarkers;

        public ObservableCollection<MapLayer> MerchantMarkers
        {
            get { return _merchantMarkers; }
            set { _merchantMarkers = value; RaisePropertyChanged(nameof(MerchantMarkers)); }
        }

        private NetworkAPI NetworkAPI { get; set; }

        public RelayCommand MapElementClickCommand { get; set; }

        private ViewModelBase _currentData;

        public ViewModelBase CurrentData
        {
            get { return _currentData; }
            set { _currentData = value; RaisePropertyChanged(); }
        }

        public MapViewModel()
        {
            NetworkAPI = new NetworkAPI();

            MapElementClickCommand = new RelayCommand((object args) => MapElementClicked(args));

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

        private void MapElementClicked(object args)
        {
            string selected = (((MapElementClickEventArgs)args).MapElements.First() as MapIcon).Title;

            Establishment establishment = Establishments.SingleOrDefault(e => e.Name == selected);

            //Debug.WriteLine(establishment.Name);

            ShowEstablishmentDialogAsync(establishment);
        }

        private async void RetrieveMerchantLocations()
        {
            Establishments = await NetworkAPI.GetAllEstablishments();
            MapIcons = new List<MapIcon>();

            foreach (Establishment e in Establishments)
            {
                MapIcons.Add(CreateMerchantMarker(e));
            }

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

        private async void ShowEstablishmentDialogAsync(Establishment e)
        {
            ContentDialog contentDialog = new ContentDialog();

            contentDialog.Title = e.Name;
            contentDialog.Content = "Dit is een test";
            contentDialog.PrimaryButtonText = "Bezoek";
            contentDialog.CloseButtonText = "Terug naar kaart";
            contentDialog.DefaultButton = ContentDialogButton.Primary;

            contentDialog.PrimaryButtonClick += NavigateToEstablishmentDetail();

            await contentDialog.ShowAsync();
        }

        private void NavigateToEstablishmentDetail(Establishment e)
        {
            CurrentData = new EstablishmentDetailViewModel() { Establishment = e };
        }

    }
}
