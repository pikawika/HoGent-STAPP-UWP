﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Timers;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace uwp_app_aalst_groep_a3.Views
{
    public sealed partial class EstablishmentDetailView : UserControl
    {
        public EstablishmentDetailView()
        {
            this.InitializeComponent();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < 700 || e.NewSize.Height < 600)
            {
                topcarousel.Visibility = Visibility.Collapsed;
            }
            else
            {
                topcarousel.Visibility = Visibility.Visible;
            }
        }
    }
}
