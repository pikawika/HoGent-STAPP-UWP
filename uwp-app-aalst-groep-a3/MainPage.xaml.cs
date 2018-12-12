using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Cortana;
using uwp_app_aalst_groep_a3.ViewModels;
using Windows.ApplicationModel.Activation;
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

namespace uwp_app_aalst_groep_a3
{
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel mainPageViewModel;

        public MainPage()
        {
            this.InitializeComponent();
            mainPageViewModel = new MainPageViewModel();
            this.DataContext = mainPageViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if ((e.Parameter as string) != null && (e.Parameter as string) != "")
            {
                CortanaFunctions cortanaFunctions = new CortanaFunctions(mainPageViewModel);

                cortanaFunctions.RunCommand(e.Parameter as string);
            }
        }
    }
}
