using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Models;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class EventDetailViewModel : ViewModelBase
    {
        public Event Event { get; set; }

        public EventDetailViewModel(Event Event)
        {
            this.Event = Event;
        }
    }
}
