using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Utils;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class RegistrationViewModel : ViewModelBase
    {
        private MainPageViewModel mainPageViewModel;

        public RelayCommand NavigateToLoginCommand { get; set; }

        public RegistrationViewModel(MainPageViewModel mainPageViewModel)
        {
            this.mainPageViewModel = mainPageViewModel;

            NavigateToLoginCommand = new RelayCommand(_ => NavigateToLogin());
        }

        private void NavigateToLogin() => mainPageViewModel.CurrentData = new LoginViewModel(mainPageViewModel);
    }
}
