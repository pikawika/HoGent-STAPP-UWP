using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Base;
using uwp_app_aalst_groep_a3.Models;
using uwp_app_aalst_groep_a3.Network;
using uwp_app_aalst_groep_a3.Utils;
using uwp_app_aalst_groep_a3.Views;
using Windows.ApplicationModel.Appointments;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class EventDetailViewModel : ViewModelBase
    {
        public Event Event { get; set; }
        private MainPageViewModel mainPageViewModel;
        private NetworkAPI networkAPI = new NetworkAPI();

        public RelayCommand ShowEstablishmentCommandClicked { get; set; }
        public RelayCommand AddToCalendarCommand { get; set; }

        private Visibility _merchantVisibility = Visibility.Collapsed;
        public Visibility MerchantVisibility
        {
            get { return _merchantVisibility; }
            set { _merchantVisibility = value; RaisePropertyChanged(nameof(MerchantVisibility)); }
        }

        public RelayCommand EditEventCommand { get; set; }
        public RelayCommand DeleteEventCommand { get; set; }

        public EventDetailViewModel(Event Event, MainPageViewModel mainPageViewModel)
        {
            this.mainPageViewModel = mainPageViewModel;
            this.Event = Event;

            ShowEstablishmentCommandClicked = new RelayCommand(async (object args) => await ShowEstablishmentAsync());
            AddToCalendarCommand = new RelayCommand((object args) => AddEventToCalendar(args));

            EditEventCommand = new RelayCommand(_ => EditEvent());
            DeleteEventCommand = new RelayCommand(async _ => await DeleteEventDialog());

            CheckMerchantOwnsEvent();
        }

        private async Task ShowEstablishmentAsync() {
            NetworkAPI networkAPI = new NetworkAPI();
            Establishment establishment = await networkAPI.GetEstablishmentById(Event.Establishment.EstablishmentId);
            mainPageViewModel.NavigateTo(new EstablishmentDetailViewModel(establishment, mainPageViewModel));
        }

        private async void AddEventToCalendar(object args)
        {
            var appointment = new Appointment();
            appointment.Subject = Event.Name;
            appointment.Details = Event.Message;
            appointment.Location = Event.Establishment.Name;
            appointment.StartTime = Event.StartDate;
            appointment.Duration = Event.EndDate - Event.StartDate;
            var rect = GetElementRect((args as Button) as FrameworkElement);
            string appointmentID = await AppointmentManager.ShowAddAppointmentAsync(appointment, rect, Placement.Default);
        }

        private async void CheckMerchantOwnsEvent()
        {
            try
            {
                var role = UserUtils.GetUserRole();
                if (role.ToLower() == "merchant")
                {
                    bool isOwner = await networkAPI.IsOwnerOfEvent(Event.EventId);

                    if (isOwner) MerchantVisibility = Visibility.Visible;
                }
            }
            catch { }
        }

        public static Rect GetElementRect(FrameworkElement element)
        {
            GeneralTransform buttonTransform = element.TransformToVisual(null);
            Point point = buttonTransform.TransformPoint(new Point());
            return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
        }

        private void EditEvent() { }

        private async Task DeleteEventDialog()
        {
            ContentDialog contentDialog = new ContentDialog();

            contentDialog.Title = "Evenement verwijderen";
            contentDialog.Content = "Bent u zeker dat u dit evenement wilt verwijderen?";
            contentDialog.PrimaryButtonText = "Ja";
            contentDialog.CloseButtonText = "Nee";

            contentDialog.PrimaryButtonCommand = new RelayCommand(async _ => await DeleteEvent());

            await contentDialog.ShowAsync();
        }

        private async Task DeleteEvent() {
            var message = await networkAPI.DeleteEvent(Event.EventId);
            await MessageUtils.ShowDialog("Evenement verwijderen", message.Item1);
            if (message.Item2)
            {
                mainPageViewModel.BackButtonPressed();
                mainPageViewModel.NavigationHistoryItems.RemoveAll(v => v.GetType() == typeof(EventDetailViewModel));
            }
        }

    }
}
