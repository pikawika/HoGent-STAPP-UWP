using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.UserActivities;
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
        UserActivitySession _currentActivity;

        public MapView()
        {
            this.InitializeComponent();
            GenerateActivityAsync();
        }

        private async void GenerateActivityAsync()
        {
            // Get the default UserActivityChannel and query it for our UserActivity. If the activity doesn't exist, one is created.
            UserActivityChannel channel = UserActivityChannel.GetDefault();
            UserActivity userActivity = await channel.GetOrCreateUserActivityAsync("ShowMap");

            // Populate required properties
            userActivity.VisualElements.DisplayText = "Kaart";
            userActivity.VisualElements.Description = "Bekijk de kaart";
            userActivity.ActivationUri = new Uri("stapp://ShowMap");

            //Save
            await userActivity.SaveAsync(); //save the new metadata

            // Dispose of any current UserActivitySession, and create a new one.
            _currentActivity?.Dispose();
            _currentActivity = userActivity.CreateSession();
        }
    }
}
