using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Base;
using uwp_app_aalst_groep_a3.Models;
using uwp_app_aalst_groep_a3.Network;
using uwp_app_aalst_groep_a3.Utils;
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

        private Visibility _companyVisibility = Visibility.Collapsed;
        public Visibility CompanyVisibility
        {
            get => _companyVisibility;
            set { _companyVisibility = value; RaisePropertyChanged(nameof(CompanyVisibility)); }
        }

        private Visibility _establishmentVisibility = Visibility.Collapsed;
        public Visibility EstablishmentVisibility
        {
            get => _establishmentVisibility;
            set { _establishmentVisibility = value; RaisePropertyChanged(nameof(EstablishmentVisibility)); }
        }

        private Visibility _promotionVisibility = Visibility.Collapsed;
        public Visibility PromotionVisibility
        {
            get => _promotionVisibility;
            set { _promotionVisibility = value; RaisePropertyChanged(nameof(PromotionVisibility)); }
        }

        private Visibility _eventVisibility = Visibility.Collapsed;
        public Visibility EventVisibility
        {
            get => _eventVisibility;
            set { _eventVisibility = value; RaisePropertyChanged(nameof(EventVisibility)); }
        }

        private string _buttonText = "";
        public string ButtonText
        {
            get => _buttonText;
            set { _buttonText = value; RaisePropertyChanged(nameof(ButtonText)); }
        }

        public RelayCommand CancelAddCommand { get; set; }
        public RelayCommand AddMerchantObjectCommand { get; set; }

        public MerchantAddViewModel(MerchantObjectType merchantObjectType, MainPageViewModel mainPageViewModel)
        {
            this.merchantObjectType = merchantObjectType;
            this.mainPageViewModel = mainPageViewModel;

            InitializeMerchantAdd();

            CancelAddCommand = new RelayCommand(_ => CancelAddDialog());
            AddMerchantObjectCommand = new RelayCommand(_ => AddMerchantObject());
        }

        private void InitializeMerchantAdd()
        {
            switch (merchantObjectType)
            {
                case MerchantObjectType.COMPANY:
                    InitializeCompany();
                    break;
                case MerchantObjectType.ESTABLISHMENT:
                    InitializeEstablishment();
                    break;
                case MerchantObjectType.PROMOTION:
                    InitializePromotion();
                    break;
                case MerchantObjectType.EVENT:
                    InitializeEvent();
                    break;
            }
        }

        private void InitializeCompany()
        {
            Company = new Company();
            ButtonText = "Voeg bedrijf toe";
            CompanyVisibility = Visibility.Visible;
        }

        private void InitializeEstablishment()
        {
            Establishment = new Establishment();
            ButtonText = "Voeg vestiging toe";
            EstablishmentVisibility = Visibility.Visible;
        }

        private void InitializePromotion()
        {
            Promotion = new Promotion();
            ButtonText = "Voeg promotie toe";
            PromotionVisibility = Visibility.Visible;
        }

        private void InitializeEvent()
        {
            Event = new Event();
            ButtonText = "Voeg evenement toe";
            EventVisibility = Visibility.Visible;
        }

        private void AddMerchantObject()
        {

        }

        private async void CancelAddDialog()
        {
            ContentDialog contentDialog = new ContentDialog();

            contentDialog.Title = "Toevoegen annuleren";
            contentDialog.Content = "Bent u zeker dat u het toevoegen wilt annuleren?";
            contentDialog.PrimaryButtonText = "Ja";
            contentDialog.CloseButtonText = "Nee";

            contentDialog.PrimaryButtonCommand = new RelayCommand(_ => CancelAdd());

            await contentDialog.ShowAsync();
        }

        private void CancelAdd() => mainPageViewModel.BackButtonPressed();

    }
}
