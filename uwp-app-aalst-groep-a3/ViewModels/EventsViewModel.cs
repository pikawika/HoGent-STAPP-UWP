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
    public class EventsViewModel : ViewModelBase
    {
        private MainPageViewModel mainPageViewModel;

        private NetworkAPI NetworkAPI = new NetworkAPI();

        private ObservableCollection<Event> _events;

        public ObservableCollection<Event> Events
        {
            get { return _events; }
            set { _events = value; RaisePropertyChanged(nameof(Events)); Loading = false; }
        }

        public RelayCommand EventClickedCommand { get; set; }

        private bool _loading = true;

        public bool Loading
        {
            get { return _loading; }
            set { _loading = value; RaisePropertyChanged(nameof(Loading)); Shown = value; }
        }

        public bool Shown
        {
            get { return !_loading; }
            set { _loading = value; RaisePropertyChanged(nameof(Shown)); }
        }

        public EventsViewModel(MainPageViewModel mainPageViewModel)
        {
            this.mainPageViewModel = mainPageViewModel;

            EventClickedCommand = new RelayCommand((object args) => EventClicked(args));

            InitializeHomePage();
        }

        private async void InitializeHomePage() => Events = new ObservableCollection<Event>(await NetworkAPI.GetAllEvents());

        private void EventClicked(object args) => mainPageViewModel.NavigateTo(new EventDetailViewModel(args as Event, mainPageViewModel));
    }
}
