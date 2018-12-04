using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Models;
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

        public RelayCommand AddToCalendarCommand { get; set; }

        public EventDetailViewModel(Event Event)
        {
            this.Event = Event;

            AddToCalendarCommand = new RelayCommand((object args) => AddEventToCalendar(args));
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

        public static Rect GetElementRect(FrameworkElement element)
        {
            GeneralTransform buttonTransform = element.TransformToVisual(null);
            Point point = buttonTransform.TransformPoint(new Point());
            return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
        }

    }
}
