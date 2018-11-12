using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace uwp_app_aalst_groep_a3.Views
{
    public sealed partial class MapView : UserControl
    {
        public MapView()
        {
            this.InitializeComponent();

            //BasicGeoposition beginPositie = new BasicGeoposition();
            //beginPositie.Latitude = startPositie.Coordinate.Latitude;
            //beginPositie.Longitude = startPositie.Coordinate.Longitude;

            //Geopoint gp1 = new Geopoint(beginPositie);

            //MapIcon mapIcon1 = new MapIcon();
            //mapIcon1.Location = gp1;
            //mapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
            //mapIcon1.Title = "Huidige locatie!";
            //mapIcon1.ZIndex = 0;
            //MyMap.MapElements.Add(mapIcon1);

            //MyMap.Center = gp1;
            //MyMap.ZoomLevel = 10;
        }
    }
}
