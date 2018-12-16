using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Base;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class MerchantPanelViewModel : ViewModelBase
    {
        private MainPageViewModel mainPageViewModel;

        public MerchantPanelViewModel(MainPageViewModel mainPageViewModel)
        {
            this.mainPageViewModel = mainPageViewModel;
        }
    }
}
