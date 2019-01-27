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

        private ObservableCollection<OpenHourForVm> _openHoursMonday = new ObservableCollection<OpenHourForVm>();
        public ObservableCollection<OpenHourForVm> OpenHoursMonday
        {
            get => _openHoursMonday;
            set { _openHoursMonday = value; RaisePropertyChanged(nameof(OpenHoursMonday)); }
        }

        private ObservableCollection<OpenHourForVm> _openHoursTuesday = new ObservableCollection<OpenHourForVm>();
        public ObservableCollection<OpenHourForVm> OpenHoursTuesday
        {
            get => _openHoursTuesday;
            set { _openHoursTuesday = value; RaisePropertyChanged(nameof(OpenHoursTuesday)); }
        }

        private ObservableCollection<OpenHourForVm> _openHoursWednesday = new ObservableCollection<OpenHourForVm>();
        public ObservableCollection<OpenHourForVm> OpenHoursWednesday
        {
            get => _openHoursWednesday;
            set { _openHoursWednesday = value; RaisePropertyChanged(nameof(OpenHoursWednesday)); }
        }

        private ObservableCollection<OpenHourForVm> _openHoursThursday = new ObservableCollection<OpenHourForVm>();
        public ObservableCollection<OpenHourForVm> OpenHoursThursday
        {
            get => _openHoursThursday;
            set { _openHoursThursday = value; RaisePropertyChanged(nameof(OpenHoursThursday)); }
        }

        private ObservableCollection<OpenHourForVm> _openHoursFriday = new ObservableCollection<OpenHourForVm>();
        public ObservableCollection<OpenHourForVm> OpenHoursFriday
        {
            get => _openHoursFriday;
            set { _openHoursFriday = value; RaisePropertyChanged(nameof(OpenHoursFriday)); }
        }

        private ObservableCollection<OpenHourForVm> _openHoursSaturday = new ObservableCollection<OpenHourForVm>();
        public ObservableCollection<OpenHourForVm> OpenHoursSaturday
        {
            get => _openHoursSaturday;
            set { _openHoursSaturday = value; RaisePropertyChanged(nameof(OpenHoursSaturday)); }
        }

        private ObservableCollection<OpenHourForVm> _openHoursSunday = new ObservableCollection<OpenHourForVm>();
        public ObservableCollection<OpenHourForVm> OpenHoursSunday
        {
            get => _openHoursSunday;
            set { _openHoursSunday = value; RaisePropertyChanged(nameof(OpenHoursSunday)); }
        }

        private ObservableCollection<Company> _companies;
        public ObservableCollection<Company> Companies
        {
            get => _companies;
            set { _companies = value; RaisePropertyChanged(nameof(Companies)); }
        }

        private ObservableCollection<CategoryRequest> _categories = new ObservableCollection<CategoryRequest>();
        public ObservableCollection<CategoryRequest> Categories
        {
            get => _categories;
            set { _categories = value; RaisePropertyChanged(nameof(Categories)); }
        }

        private ObservableCollection<ExceptionalDayRequest> _exceptionalDays = new ObservableCollection<ExceptionalDayRequest>();
        public ObservableCollection<ExceptionalDayRequest> ExceptionalDays
        {
            get => _exceptionalDays;
            set { _exceptionalDays = value; RaisePropertyChanged(nameof(ExceptionalDays)); }
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
        public RelayCommand AddCategoryCommand { get; set; }
        public RelayCommand DeleteCategoryCommand { get; set; }
        public RelayCommand AddExceptionalDayCommand { get; set; }
        public RelayCommand DeleteExceptionalDayCommand { get; set; }
        public RelayCommand AddOpenHoursCommand { get; set; }
        public RelayCommand DeleteOpenHoursCommand { get; set; }
        public RelayCommand AddMerchantObjectCommand { get; set; }

        public MerchantAddViewModel(MerchantObjectType merchantObjectType, MainPageViewModel mainPageViewModel)
        {
            this.merchantObjectType = merchantObjectType;
            this.mainPageViewModel = mainPageViewModel;

            InitializeMerchantAddAsync();

            CancelAddCommand = new RelayCommand(_ => CancelAddDialog());

            AddCategoryCommand = new RelayCommand(_ => AddCategoryField());
            DeleteCategoryCommand = new RelayCommand(_ => DeleteCategoryField());

            AddExceptionalDayCommand = new RelayCommand(_ => AddExceptionalDayField());
            DeleteExceptionalDayCommand = new RelayCommand(_ => DeleteExceptionalDayField());

            AddOpenHoursCommand = new RelayCommand((object args) => AddOpenHourField(args));
            DeleteOpenHoursCommand = new RelayCommand((object args) => DeleteOpenHourField(args));

            List<FileExtension> imageExtensions = new List<FileExtension>();
            imageExtensions.Add(new FileExtension { Extension = ".jpg" });
            PickImageCommand = new RelayCommand(_ => ImagePickerDialog(imageExtensions));

            List<FileExtension> attachmentsExtensions = new List<FileExtension>();
            attachmentsExtensions.Add(new FileExtension { Extension = ".pdf" });
            PickAttachmentCommand = new RelayCommand(_ => AttachmentPickerDialog(attachmentsExtensions));

            AddMerchantObjectCommand = new RelayCommand(async _ => await AddMerchantObject());


        }

        private async void InitializeMerchantAddAsync()
        {
            switch (merchantObjectType)
            {
                case MerchantObjectType.COMPANY:
                    InitializeCompany();
                    break;
                case MerchantObjectType.ESTABLISHMENT:
                    await InitializeEstablishment();
                    break;
                case MerchantObjectType.PROMOTION:
                    await InitializePromotion();
                    break;
                case MerchantObjectType.EVENT:
                    await InitializeEvent();
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

                    ExceptionalDays.Add(new ExceptionalDayRequest() { Day = DateTime.Today, Message = "" });

                    Categories.Add(new CategoryRequest { Name = "" });

                    OpenHoursMonday.Add(new OpenHourForVm());
                    OpenHoursTuesday.Add(new OpenHourForVm());
                    OpenHoursWednesday.Add(new OpenHourForVm());
                    OpenHoursThursday.Add(new OpenHourForVm());
                    OpenHoursFriday.Add(new OpenHourForVm());
                    OpenHoursSaturday.Add(new OpenHourForVm());
                    OpenHoursSunday.Add(new OpenHourForVm());
                }
                else
                {
                    var message = await networkAPI.AddEvent(Event);
                    await MessageUtils.ShowDialog("Gelieve eerst een bedrijf toe te voegen", message.Item1);
                    if (message.Item2)
                    {
                        mainPageViewModel.BackButtonPressed();
                        mainPageViewModel.NavigationHistoryItems.RemoveAll(v => v.GetType() == typeof(MerchantAddViewModel));
                    }
                }


            }
            else
            {
                var message = await networkAPI.AddEvent(Event);
                await MessageUtils.ShowDialog("Gelieve eerst een company toe te voegen", message.Item1);
                if (message.Item2)
                {
                    mainPageViewModel.BackButtonPressed();
                    mainPageViewModel.NavigationHistoryItems.RemoveAll(v => v.GetType() == typeof(MerchantAddViewModel));
                }
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
                else
                {
                    var message = await networkAPI.AddEvent(Event);
                    await MessageUtils.ShowDialog("Gelieve eerst een vestiging toe te voegen", message.Item1);
                    if (message.Item2)
                    {
                        mainPageViewModel.BackButtonPressed();
                        mainPageViewModel.NavigationHistoryItems.RemoveAll(v => v.GetType() == typeof(MerchantAddViewModel));
                    }
                }
            }
            else
            {
                var message = await networkAPI.AddEvent(Event);
                await MessageUtils.ShowDialog("Gelieve eerst een vestiging toe te voegen", message.Item1);
                if (message.Item2)
                {
                    mainPageViewModel.BackButtonPressed();
                    mainPageViewModel.NavigationHistoryItems.RemoveAll(v => v.GetType() == typeof(MerchantAddViewModel));
                }
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
                else
                {
                    var message = await networkAPI.AddEvent(Event);
                    await MessageUtils.ShowDialog("Gelieve eerst een vestiging toe te voegen", message.Item1);
                    if (message.Item2)
                    {
                        mainPageViewModel.BackButtonPressed();
                        mainPageViewModel.NavigationHistoryItems.RemoveAll(v => v.GetType() == typeof(MerchantAddViewModel));
                    }
                }
            }
            else
            {
                var message = await networkAPI.AddEvent(Event);
                await MessageUtils.ShowDialog("Gelieve eerst een vestiging toe te voegen", message.Item1);
                if (message.Item2)
                {
                    mainPageViewModel.BackButtonPressed();
                    mainPageViewModel.NavigationHistoryItems.RemoveAll(v => v.GetType() == typeof(MerchantAddViewModel));
                }
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
                    mainPageViewModel.BackButtonPressed();
                    mainPageViewModel.NavigationHistoryItems.RemoveAll(v => v.GetType() == typeof(MerchantAddViewModel) || v.GetType() == typeof(MerchantPanelViewModel));
                    mainPageViewModel.NavigateTo(new MerchantPanelViewModel(mainPageViewModel));
                }
            }

            if (Establishment != null)
            {
                if (!string.IsNullOrEmpty(Facebook))
                {
                    Establishment.SocialMedias.Add(new SocialMediaRequest { Name = "facebook", Url = Facebook });
                }

                if (!string.IsNullOrEmpty(Twitter))
                {
                    Establishment.SocialMedias.Add(new SocialMediaRequest { Name = "twitter", Url = Twitter });
                }

                if (!string.IsNullOrEmpty(Instagram))
                {
                    Establishment.SocialMedias.Add(new SocialMediaRequest { Name = "instagram", Url = Instagram });
                }

                Establishment.CompanyId = PickedCompany.CompanyId;

                var categories = Categories.ToList();
                categories.RemoveAll(c => string.IsNullOrEmpty(c.Name));
                Establishment.Categories = categories;

                var exceptionalDays = ExceptionalDays.ToList();
                exceptionalDays.RemoveAll(ed => string.IsNullOrEmpty(ed.Message));
                Establishment.ExceptionalDays = exceptionalDays;

                var openHoursVm0 = OpenHoursMonday.ToList();
                openHoursVm0.RemoveAll(oh => oh.IsClosed);
                var openDays0 = new OpenDayRequest{DayOfTheWeek = 0};
                foreach (var openHourVm in openHoursVm0)
                {
                    openDays0.OpenHours.Add(new OpenHourRequest{StartHour = openHourVm.StartTime.Hours, EndHour = openHourVm.EndTime.Hours, Startminute = openHourVm.StartTime.Minutes, EndMinute = openHourVm.EndTime.Minutes });
                }
                Establishment.OpenDays.Add(openDays0);

                var openHoursVm1 = OpenHoursTuesday.ToList();
                openHoursVm1.RemoveAll(oh => oh.IsClosed);
                var openDays1 = new OpenDayRequest { DayOfTheWeek = 1 };
                foreach (var openHourVm in openHoursVm1)
                {
                    openDays1.OpenHours.Add(new OpenHourRequest { StartHour = openHourVm.StartTime.Hours, EndHour = openHourVm.EndTime.Hours, Startminute = openHourVm.StartTime.Minutes, EndMinute = openHourVm.EndTime.Minutes });
                }
                Establishment.OpenDays.Add(openDays1);

                var openHoursVm2 = OpenHoursWednesday.ToList();
                openHoursVm2.RemoveAll(oh => oh.IsClosed);
                var openDays2 = new OpenDayRequest { DayOfTheWeek = 2 };
                foreach (var openHourVm in openHoursVm2)
                {
                    openDays2.OpenHours.Add(new OpenHourRequest { StartHour = openHourVm.StartTime.Hours, EndHour = openHourVm.EndTime.Hours, Startminute = openHourVm.StartTime.Minutes, EndMinute = openHourVm.EndTime.Minutes });
                }
                Establishment.OpenDays.Add(openDays2);

                var openHoursVm3 = OpenHoursThursday.ToList();
                openHoursVm3.RemoveAll(oh => oh.IsClosed);
                var openDays3 = new OpenDayRequest { DayOfTheWeek = 3 };
                foreach (var openHourVm in openHoursVm3)
                {
                    openDays3.OpenHours.Add(new OpenHourRequest { StartHour = openHourVm.StartTime.Hours, EndHour = openHourVm.EndTime.Hours, Startminute = openHourVm.StartTime.Minutes, EndMinute = openHourVm.EndTime.Minutes });
                }
                Establishment.OpenDays.Add(openDays3);

                var openHoursVm4 = OpenHoursFriday.ToList();
                openHoursVm4.RemoveAll(oh => oh.IsClosed);
                var openDays4 = new OpenDayRequest { DayOfTheWeek = 4 };
                foreach (var openHourVm in openHoursVm4)
                {
                    openDays4.OpenHours.Add(new OpenHourRequest { StartHour = openHourVm.StartTime.Hours, EndHour = openHourVm.EndTime.Hours, Startminute = openHourVm.StartTime.Minutes, EndMinute = openHourVm.EndTime.Minutes });
                }
                Establishment.OpenDays.Add(openDays4);

                var openHoursVm5 = OpenHoursSaturday.ToList();
                openHoursVm5.RemoveAll(oh => oh.IsClosed);
                var openDays5 = new OpenDayRequest { DayOfTheWeek = 5 };
                foreach (var openHourVm in openHoursVm5)
                {
                    openDays5.OpenHours.Add(new OpenHourRequest { StartHour = openHourVm.StartTime.Hours, EndHour = openHourVm.EndTime.Hours, Startminute = openHourVm.StartTime.Minutes, EndMinute = openHourVm.EndTime.Minutes });
                }
                Establishment.OpenDays.Add(openDays5);

                var openHoursVm6 = OpenHoursSunday.ToList();
                openHoursVm6.RemoveAll(oh => oh.IsClosed);
                var openDays6 = new OpenDayRequest { DayOfTheWeek = 6 };
                foreach (var openHourVm in openHoursVm6)
                {
                    openDays6.OpenHours.Add(new OpenHourRequest { StartHour = openHourVm.StartTime.Hours, EndHour = openHourVm.EndTime.Hours, Startminute = openHourVm.StartTime.Minutes, EndMinute = openHourVm.EndTime.Minutes });
                }
                Establishment.OpenDays.Add(openDays6);

                var message = await networkAPI.AddEstablishment(Establishment);
                await MessageUtils.ShowDialog("Vestiging toevoegen", message.Item1);
                if (message.Item2)
                {
                    mainPageViewModel.BackButtonPressed();
                    mainPageViewModel.BackButtonPressed();
                    mainPageViewModel.NavigationHistoryItems.RemoveAll(v => v.GetType() == typeof(MerchantAddViewModel) || v.GetType() == typeof(MerchantPanelViewModel));
                    mainPageViewModel.NavigateTo(new MerchantPanelViewModel(mainPageViewModel));
                }

            }

            if (Promotion != null)
            {
                Promotion.EstablishmentId = PickedEstablishment.EstablishmentId;
                var message = await networkAPI.AddPromotion(Promotion);
                await MessageUtils.ShowDialog("Promotie toevoegen", message.Item1);
                if (message.Item2)
                {
                    mainPageViewModel.BackButtonPressed();
                    mainPageViewModel.BackButtonPressed();
                    mainPageViewModel.NavigationHistoryItems.RemoveAll(v => v.GetType() == typeof(MerchantAddViewModel) || v.GetType() == typeof(MerchantPanelViewModel));
                    mainPageViewModel.NavigateTo(new MerchantPanelViewModel(mainPageViewModel));
                }
            }

            if (Event != null)
            {
                Event.EstablishmentId = PickedEstablishment.EstablishmentId;
                var message = await networkAPI.AddEvent(Event);
                await MessageUtils.ShowDialog("Event toevoegen", message.Item1);
                if (message.Item2)
                {
                    mainPageViewModel.BackButtonPressed();
                    mainPageViewModel.BackButtonPressed();
                    mainPageViewModel.NavigationHistoryItems.RemoveAll(v => v.GetType() == typeof(MerchantAddViewModel) || v.GetType() == typeof(MerchantPanelViewModel));
                    mainPageViewModel.NavigateTo(new MerchantPanelViewModel(mainPageViewModel));
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

        private void AddCategoryField()
        {
            _categories.Add(new CategoryRequest { Name = "" });
        }

        private void DeleteCategoryField()
        {
            _categories.Clear();
            _categories.Add(new CategoryRequest { Name = "" });
        }

        private void AddOpenHourField(object args)
        {
            var dayOfWeek = int.Parse(args as string);

            switch (dayOfWeek)
            {
                case 0:
                    OpenHoursMonday.Add(new OpenHourForVm());
                    break;
                case 1:
                    OpenHoursTuesday.Add(new OpenHourForVm());
                    break;
                case 2:
                    OpenHoursWednesday.Add(new OpenHourForVm());
                    break;
                case 3:
                    OpenHoursThursday.Add(new OpenHourForVm());
                    break;
                case 4:
                    OpenHoursFriday.Add(new OpenHourForVm());
                    break;
                case 5:
                    OpenHoursSaturday.Add(new OpenHourForVm());
                    break;
                case 6:
                    OpenHoursSunday.Add(new OpenHourForVm());
                    break;
            }
        }

        private void DeleteOpenHourField(object args)
        {
            var dayOfWeek = int.Parse(args as string);


            switch (dayOfWeek)
            {
                case 0:
                    OpenHoursMonday.Clear();
                    OpenHoursMonday.Add(new OpenHourForVm());
                    break;
                case 1:
                    OpenHoursTuesday.Clear();
                    OpenHoursTuesday.Add(new OpenHourForVm());
                    break;
                case 2:
                    OpenHoursWednesday.Clear();
                    OpenHoursWednesday.Add(new OpenHourForVm());
                    break;
                case 3:
                    OpenHoursThursday.Clear();
                    OpenHoursThursday.Add(new OpenHourForVm());
                    break;
                case 4:
                    OpenHoursFriday.Clear();
                    OpenHoursFriday.Add(new OpenHourForVm());
                    break;
                case 5:
                    OpenHoursSaturday.Clear();
                    OpenHoursSaturday.Add(new OpenHourForVm());
                    break;
                case 6:
                    OpenHoursSunday.Clear();
                    OpenHoursSunday.Add(new OpenHourForVm());
                    break;
            }
        }

        private void AddExceptionalDayField()
        {
            _exceptionalDays.Add(new ExceptionalDayRequest() { Day = DateTime.Today, Message = "" });
        }

        private void DeleteExceptionalDayField()
        {
            _exceptionalDays.Clear();
            _exceptionalDays.Add(new ExceptionalDayRequest() { Day = DateTime.Today, Message = "" });
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
