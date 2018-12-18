using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using uwp_app_aalst_groep_a3.Base;
using uwp_app_aalst_groep_a3.Models;
using uwp_app_aalst_groep_a3.Network;
using uwp_app_aalst_groep_a3.Utils;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using uwp_app_aalst_groep_a3.Network.requests;
using uwp_app_aalst_groep_a3.Network.Request.Attachments;
using uwp_app_aalst_groep_a3.Network.Request.Establishment;
using uwp_app_aalst_groep_a3.Network.Request.Event;
using uwp_app_aalst_groep_a3.Network.Request.Promotion;

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

        private EstablishmentRequest _establishment;
        public EstablishmentRequest Establishment
        {
            get => _establishment;
            set { _establishment = value; RaisePropertyChanged(nameof(Establishment)); }
        }

        private PromotionRequest _promotion;
        public PromotionRequest Promotion
        {
            get => _promotion;
            set { _promotion = value; RaisePropertyChanged(nameof(Promotion)); }
        }

        private EventRequest _event;
        public EventRequest Event
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

        private string _facebook;
        public string Facebook
        {
            get => _facebook;
            set { _facebook = value; RaisePropertyChanged(nameof(Facebook)); }
        }

        private string _twitter;
        public string Twitter
        {
            get => _twitter;
            set { _twitter = value; RaisePropertyChanged(nameof(Twitter)); }
        }

        private string _instagram;
        public string Instagram
        {
            get => _instagram;
            set { _instagram = value; RaisePropertyChanged(nameof(Instagram)); }
        }

        private ObservableCollection<Company> _companies;
        public ObservableCollection<Company> Companies
        {
            get => _companies;
            set { _companies = value; RaisePropertyChanged(nameof(Companies)); }
        }

        private Company _pickedCompany;
        public Company PickedCompany
        {
            get => _pickedCompany;
            set { _pickedCompany = value; RaisePropertyChanged(nameof(PickedCompany)); }
        }

        private ObservableCollection<Establishment> _establishments;
        public ObservableCollection<Establishment> Establishments
        {
            get => _establishments;
            set { _establishments = value; RaisePropertyChanged(nameof(Establishments)); }
        }

        private Establishment _pickedEstablishment;
        public Establishment PickedEstablishment
        {
            get => _pickedEstablishment;
            set { _pickedEstablishment = value; RaisePropertyChanged(nameof(PickedEstablishment)); }
        }

        private string _buttonText = "";
        public string ButtonText
        {
            get => _buttonText;
            set { _buttonText = value; RaisePropertyChanged(nameof(ButtonText)); }
        }

        public RelayCommand PickImageCommand { get; set; }
        public RelayCommand PickAttachmentCommand { get; set; }
        public RelayCommand CancelAddCommand { get; set; }
        public RelayCommand AddMerchantObjectCommand { get; set; }

        public MerchantAddViewModel(MerchantObjectType merchantObjectType, MainPageViewModel mainPageViewModel)
        {
            this.merchantObjectType = merchantObjectType;
            this.mainPageViewModel = mainPageViewModel;

            InitializeMerchantAdd();

            CancelAddCommand = new RelayCommand(_ => CancelAddDialog());

            List<FileExtension> imageExtensions = new List<FileExtension>();
            imageExtensions.Add(new FileExtension { Extension = ".jpg" });
            PickImageCommand = new RelayCommand(_ => ImagePickerDialog(imageExtensions));

            List<FileExtension> attachmentsExtensions = new List<FileExtension>();
            attachmentsExtensions.Add(new FileExtension { Extension = ".pdf" });
            PickAttachmentCommand = new RelayCommand(_ => AttachmentPickerDialog(attachmentsExtensions));

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

        private async Task InitializeEstablishment()
        {
            Establishment = new EstablishmentRequest();
            ButtonText = "Voeg vestiging toe";
            EstablishmentVisibility = Visibility.Visible;

            //lijst van companies
            Companies = new ObservableCollection<Company>();

            //selected company
            PickedCompany = new Company();

            var result = await networkAPI.GetCompanies();

            if (result.Item2 == null)
            {
                Companies = new ObservableCollection<Company>(result.Item1);
                if (Companies.Any())
                {
                    PickedCompany = Companies[0];
                }
            }
            else
            {
                //TODO nog geen companie voegt da keer toe
            }

            
        }

        private async Task InitializePromotion()
        {
            Promotion = new PromotionRequest();
            ButtonText = "Voeg promotie toe";
            PromotionVisibility = Visibility.Visible;

            //lijst van establishments
            Establishments = new ObservableCollection<Establishment>();

            //selected establishment
            PickedEstablishment = new Establishment();

            var result = await networkAPI.GetCompanies();

            if (result.Item2 == null)
            {
                Establishments = new ObservableCollection<Establishment>();

                foreach (Company company in result.Item1)
                {
                    foreach (Establishment establishment in company.Establishments)
                    {
                        _establishments.Add(establishment);
                    }
                }
                if (Establishments.Any())
                {
                    PickedEstablishment = Establishments[0];
                }
            }
            else
            {
                //TODO nog geen establishment voegt da keer toe
            }
        }

        private async Task InitializeEvent()
        {
            Event = new EventRequest();
            ButtonText = "Voeg evenement toe";
            EventVisibility = Visibility.Visible;

            //lijst van establishments
            Establishments = new ObservableCollection<Establishment>();

            //selected establishment
            PickedEstablishment = new Establishment();

            var result = await networkAPI.GetCompanies();

            if (result.Item2 == null)
            {
                Establishments = new ObservableCollection<Establishment>();

                foreach (Company company in result.Item1)
                {
                    foreach (Establishment establishment in company.Establishments)
                    {
                        _establishments.Add(establishment);
                    }
                }
                if (Establishments.Any())
                {
                    PickedEstablishment = Establishments[0];
                }
            }
            else
            {
                //TODO nog geen establishment voegt da keer toe
            }
        }

        private async Task AddMerchantObject()
        {
            if (Company != null)
            {
                var message = await networkAPI.AddCompany(Company.Name);
                await MessageUtils.ShowDialog("Bedrijf toevoegen", message.Item1);
                if (message.Item2)
                {
                    mainPageViewModel.BackButtonPressed();
                    mainPageViewModel.NavigationHistoryItems.RemoveAll(v => v.GetType() == typeof(MerchantAddViewModel));
                }
            }

            if (Establishment != null)
            {
                if (Facebook != null)
                {
                    Establishment.SocialMedias.Add(new SocialMediaRequest{Name = "facebook", Url = Facebook});
                }

                if (Twitter != null)
                {
                    Establishment.SocialMedias.Add(new SocialMediaRequest { Name = "twitter", Url = Twitter });
                }

                if (Instagram != null)
                {
                    Establishment.SocialMedias.Add(new SocialMediaRequest { Name = "instagram", Url = Instagram });
                }

                Establishment.CompanyId = PickedCompany.CompanyId;
            }

            if (Promotion != null)
            {
                var message = await networkAPI.AddPromotion(Promotion);
                await MessageUtils.ShowDialog("Promotie toevoegen", message.Item1);
                if (message.Item2)
                {
                    mainPageViewModel.BackButtonPressed();
                    mainPageViewModel.NavigationHistoryItems.RemoveAll(v => v.GetType() == typeof(MerchantAddViewModel));
                }
            }

            if (Event != null)
            {
                var message = await networkAPI.AddEvent(Event);
                await MessageUtils.ShowDialog("Event toevoegen", message.Item1);
                if (message.Item2)
                {
                    mainPageViewModel.BackButtonPressed();
                    mainPageViewModel.NavigationHistoryItems.RemoveAll(v => v.GetType() == typeof(MerchantAddViewModel));
                }
            }
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

        private async void ImagePickerDialog(List<FileExtension> extensions)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            foreach (FileExtension anExtension in extensions)
            {
                picker.FileTypeFilter.Add(anExtension.Extension);
            }

            var files = await picker.PickMultipleFilesAsync();
            if (files.Count > 0)
            {
                foreach (StorageFile file in files)
                {
                    Establishment?.Images.Add(new FileRequest { Name = "picture", FullFileName = file.Name, Base64File = await FileToBase64(file) });

                    Promotion?.Images.Add(new FileRequest { Name = "picture", FullFileName = file.Name, Base64File = await FileToBase64(file) });

                    Event?.Images.Add(new FileRequest { Name = "picture", FullFileName = file.Name, Base64File = await FileToBase64(file) });
                }
            }
        }

        private async void AttachmentPickerDialog(List<FileExtension> extensions)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            foreach (FileExtension anExtension in extensions)
            {
                picker.FileTypeFilter.Add(anExtension.Extension);
            }

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                Promotion?.Attachments.Add(new FileRequest { Name = "attachment", FullFileName = file.Name, Base64File = await FileToBase64(file) });

                Event?.Attachments.Add(new FileRequest { Name = "attachment", FullFileName = file.Name, Base64File = await FileToBase64(file) });
            }
        }

        private void CancelAdd() => mainPageViewModel.BackButtonPressed();

        private async Task<string> FileToBase64(StorageFile file)
        {
            string base64String = "";

            if (file != null)
            {
                IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read);
                var reader = new DataReader(fileStream.GetInputStreamAt(0));
                await reader.LoadAsync((uint)fileStream.Size);
                byte[] byteArray = new byte[fileStream.Size];
                reader.ReadBytes(byteArray);
                base64String = Convert.ToBase64String(byteArray);
            }

            return base64String;
        }

    }
}
