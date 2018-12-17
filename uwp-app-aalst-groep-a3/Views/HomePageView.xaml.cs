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
using Microsoft.Toolkit.Uwp;
using uwp_app_aalst_groep_a3.Models;
using uwp_app_aalst_groep_a3.Network;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.UserActivities;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace uwp_app_aalst_groep_a3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePageView : UserControl
    {
        UserActivitySession _currentActivity;

        public HomePageView()
        {
            this.InitializeComponent();
            GenerateActivityAsync();
        }

        private async void GenerateActivityAsync()
        {
            // Get the default UserActivityChannel and query it for our UserActivity. If the activity doesn't exist, one is created.
            UserActivityChannel channel = UserActivityChannel.GetDefault();
            UserActivity userActivity = await channel.GetOrCreateUserActivityAsync("ShowHomePage");

            // Populate required properties
            userActivity.VisualElements.DisplayText = "Hoofdpagina";
            userActivity.VisualElements.Description = "Bekijk de hoofdpagina";
            userActivity.ActivationUri = new Uri("stapp://ShowHomePage");

            //Save
            await userActivity.SaveAsync(); //save the new metadata

            // Dispose of any current UserActivitySession, and create a new one.
            _currentActivity?.Dispose();
            _currentActivity = userActivity.CreateSession();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(e.NewSize.Width < 700)
            {
                topcarousel.Visibility = Visibility.Collapsed;
                bottompart.Height = new GridLength(500);
            }
            else
            {
                topcarousel.Visibility = Visibility.Visible;
                bottompart.Height = new GridLength(300);
            }
        }
    }
}
