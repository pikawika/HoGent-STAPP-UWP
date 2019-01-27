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
    public class MerchantEditViewModel : ViewModelBase
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
        public RelayCommand DeleteClickedCommand { get; set; }
        public RelayCommand AddMerchantObjectCommand { get; set; }

        public MerchantEditViewModel(MerchantObjectType merchantObjectType, MainPageViewModel mainPageViewModel, Company company = null, Establishment establishment = null, Promotion promotion = null, Event evenement = null)
        {
            this.merchantObjectType = merchantObjectType;
            this.mainPageViewModel = mainPageViewModel;

            InitializeMerchantAdd(company, establishment, promotion, evenement);

            CancelAddCommand = new RelayCommand(_ => CancelAddDialog());
            DeleteClickedCommand = new RelayCommand(async _ => await DeleteClickedDialog());
            AddMerchantObjectCommand = new RelayCommand(_ => AddMerchantObject());
        }

        private void InitializeMerchantAdd(Company company = null, Establishment establishment = null, Promotion promotion = null, Event evenement = null)
        {
            switch (merchantObjectType)
            {
                case MerchantObjectType.COMPANY:
                    InitializeCompany(company);
                    break;
                case MerchantObjectType.ESTABLISHMENT:
                    InitializeEstablishment(establishment);
                    break;
                case MerchantObjectType.PROMOTION:
                    InitializePromotion(promotion);
                    break;
                case MerchantObjectType.EVENT:
                    InitializeEvent(evenement);
                    break;
            }
        }

        private void InitializeCompany(Company company)
        {
            Company = company;
            ButtonText = "Bewerk bedrijf";
            CompanyVisibility = Visibility.Visible;
        }

        private void InitializeEstablishment(Establishment establishment)
        {
            Establishment = establishment;
            ButtonText = "Bewerk vestiging";
            EstablishmentVisibility = Visibility.Visible;
        }

        private void InitializePromotion(Promotion promotion)
        {
            Promotion = promotion;
            ButtonText = "Bewerk promotie";
            PromotionVisibility = Visibility.Visible;
        }

        private void InitializeEvent(Event evenement)
        {
            Event = evenement;
            ButtonText = "Bewerk evenement";
            EventVisibility = Visibility.Visible;
        }

        private void AddMerchantObject()
        {

        }

        private async void CancelAddDialog()
        {
            ContentDialog contentDialog = new ContentDialog();

            contentDialog.Title = "Bewerken annuleren";
            contentDialog.Content = "Bent u zeker dat u het bewerken wilt annuleren?";
            contentDialog.PrimaryButtonText = "Ja";
            contentDialog.CloseButtonText = "Nee";

            contentDialog.PrimaryButtonCommand = new RelayCommand(_ => CancelAdd());

            await contentDialog.ShowAsync();
        }

        private void CancelAdd() => mainPageViewModel.BackButtonPressed();

        private async Task DeleteClickedDialog()
        {
            ContentDialog contentDialog = new ContentDialog();

            contentDialog.Title = "Verwijderen";
            contentDialog.Content = "Bent u zeker dat u deze gegevens wilt verwijderen?";
            contentDialog.PrimaryButtonText = "Ja";
            contentDialog.CloseButtonText = "Nee";

            contentDialog.PrimaryButtonCommand = new RelayCommand(async _ => await DeleteClicked());

            await contentDialog.ShowAsync();
        }

        private async Task DeleteClicked()
        {
            switch (merchantObjectType)
            {
                case MerchantObjectType.COMPANY:
                    await DeleteCompany();
                    break;
                case MerchantObjectType.ESTABLISHMENT:
                    await DeleteEstablishment();
                    break;
                case MerchantObjectType.PROMOTION:
                    await DeletePromotion();
                    break;
                case MerchantObjectType.EVENT:
                    await DeleteEvent();
                    break;
            }
        }

        private async Task DeleteCompany()
        {
            var message = await networkAPI.DeleteCompany(Company.CompanyId);
            await MessageUtils.ShowDialog("Bedrijf verwijderen", message.Item1);
            if (message.Item2)
            {
                mainPageViewModel.BackButtonPressed();
                mainPageViewModel.NavigationHistoryItems.RemoveAll(v => v.GetType() == typeof(MerchantEditViewModel));
            }
        }

        private async Task DeleteEstablishment()
        {
            var message = await networkAPI.DeleteEstablishment(Establishment.EstablishmentId);
            await MessageUtils.ShowDialog("Vestiging verwijderen", message.Item1);
            if (message.Item2)
            {
                mainPageViewModel.BackButtonPressed();
                mainPageViewModel.NavigationHistoryItems.RemoveAll(v => v.GetType() == typeof(MerchantEditViewModel));
            }
        }

        private async Task DeletePromotion()
        {
            var message = await networkAPI.DeletePromotion(Promotion.PromotionId);
            await MessageUtils.ShowDialog("Promotie verwijderen", message.Item1);
            if (message.Item2)
            {
                mainPageViewModel.BackButtonPressed();
                mainPageViewModel.NavigationHistoryItems.RemoveAll(v => v.GetType() == typeof(MerchantEditViewModel));
            }
        }

        private async Task DeleteEvent()
        {
            var message = await networkAPI.DeleteEvent(Event.EventId);
            await MessageUtils.ShowDialog("Evenement verwijderen", message.Item1);
            if (message.Item2)
            {
                mainPageViewModel.BackButtonPressed();
                mainPageViewModel.NavigationHistoryItems.RemoveAll(v => v.GetType() == typeof(MerchantEditViewModel));
            }
        }

    }
}
