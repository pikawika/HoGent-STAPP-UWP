using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Base;
using uwp_app_aalst_groep_a3.Models;
using uwp_app_aalst_groep_a3.Network;
using uwp_app_aalst_groep_a3.Utils;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class PromotionsViewModel : ViewModelBase
    {
        private MainPageViewModel mainPageViewModel;

        private NetworkAPI NetworkAPI = new NetworkAPI();

        private ObservableCollection<Promotion> _promotions;

        public ObservableCollection<Promotion> Promotions
        {
            get { return _promotions; }
            set { _promotions = value; RaisePropertyChanged(nameof(Promotions)); }
        }

        public RelayCommand PromotionClickedCommand { get; set; }

        public PromotionsViewModel(MainPageViewModel mainPageViewModel)
        {
            this.mainPageViewModel = mainPageViewModel;

            PromotionClickedCommand = new RelayCommand((object args) => PromotionClicked(args));

            InitializeHomePage();
        }

        private async void InitializeHomePage() => Promotions = new ObservableCollection<Promotion>(await NetworkAPI.GetAllPromotions());

        private void PromotionClicked(object args) => mainPageViewModel.NavigateTo(new PromotionDetailViewModel(args as Promotion, mainPageViewModel));
    }
}
