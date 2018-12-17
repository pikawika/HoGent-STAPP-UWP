using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Base;
using uwp_app_aalst_groep_a3.Models;
using uwp_app_aalst_groep_a3.Network;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class MerchantAddViewModel : ViewModelBase
    {
        private NetworkAPI networkAPI = new NetworkAPI();
        private MerchantObjectType merchantObjectType;
        private MainPageViewModel mainPageViewModel;

        private Company _company;
        public Company Company
        {
            get => _company;
            set { _company = value; RaisePropertyChanged(nameof(Company)); }
        }

        private Establishment _establishment;
        public Establishment Establishment
        {
            get => _establishment;
            set { _establishment = value; RaisePropertyChanged(nameof(Establishment)); }
        }

        private Promotion _promotion;
        public Promotion Promotion
        {
            get => _promotion;
            set { _promotion = value; RaisePropertyChanged(nameof(Promotion)); }
        }

        private Event _event;
        public Event Event
        {
            get => _event;
            set { _event = value; RaisePropertyChanged(nameof(Event)); }
        }

        private string _buttonText = "";
        public string ButtonText
        {
            get => _buttonText;
            set { _buttonText = value; RaisePropertyChanged(nameof(ButtonText)); }
        }

        private Visibility _companyVisibility = Visibility.Collapsed;
        public Visibility CompanyVisibility
        {
            get => _companyVisibility;
            set { _companyVisibility = value; RaisePropertyChanged(nameof(CompanyVisibility)); }
        }

        public MerchantAddViewModel(MerchantObjectType merchantObjectType, MainPageViewModel mainPageViewModel)
        {
            this.merchantObjectType = merchantObjectType;
            this.mainPageViewModel = mainPageViewModel;

            InitializeMerchantAdd();
        }

        private void InitializeMerchantAdd()
        {
            switch (merchantObjectType)
            {
                case MerchantObjectType.COMPANY:
                    InitializeCompany();
                    break;
            }
        }

        private void InitializeCompany()
        {
            
        }
    }
}
