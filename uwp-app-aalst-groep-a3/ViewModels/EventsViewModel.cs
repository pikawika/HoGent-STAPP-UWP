using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class EventsViewModel : ViewModelBase
    {
        private MainPageViewModel mainPageViewModel;

        public EventsViewModel(MainPageViewModel mainPageViewModel)
        {
            this.mainPageViewModel = mainPageViewModel;
        }
    }
}
