﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Base;
using uwp_app_aalst_groep_a3.Models;
using uwp_app_aalst_groep_a3.Models.Domain;
using uwp_app_aalst_groep_a3.Network;
using uwp_app_aalst_groep_a3.Utils;
using Windows.UI.Notifications;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private ObservableCollection<Promotion> _promotions;

        public ObservableCollection<Promotion> Promotions
        {
            get { return _promotions; }
            set { _promotions = value; RaisePropertyChanged(nameof(Promotions));  }
        }

        private ObservableCollection<Establishment> _establishments;
        private MainPageViewModel mainPageViewModel;

        public ObservableCollection<Establishment> Establishments
        {
            get { return _establishments; }
            set { _establishments = value; RaisePropertyChanged(nameof(Establishments)); Loading = false;  }
        }

        private NetworkAPI NetworkAPI { get; set; }

        public RelayCommand EstablishmentClickedCommand { get; set; }

        private bool _loading = true;

        public bool Loading
        {
            get { return _loading; }
            set { _loading = value; RaisePropertyChanged(nameof(Loading)); Shown = value; }
        }

        public bool Shown
        {
            get { return !_loading; }
            set { _loading = value; RaisePropertyChanged(nameof(Shown)); }
        }

        public HomePageViewModel(MainPageViewModel mainPageViewModel)
        {
            this.mainPageViewModel = mainPageViewModel;
            EstablishmentClickedCommand = new RelayCommand((object args) => EstablishmentClicked(args));
            NetworkAPI = new NetworkAPI();
            InitializeHomePage();
        }

        private async void InitializeHomePage()
        {
            Promotions = new ObservableCollection<Promotion>(await NetworkAPI.GetAllPromotions());
            Establishments = new ObservableCollection<Establishment>(await NetworkAPI.GetAllEstablishments());
        }

        private void EstablishmentClicked(object args) => mainPageViewModel.NavigateTo(new EstablishmentDetailViewModel(args as Establishment, mainPageViewModel));

    }
}
