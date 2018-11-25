using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Models;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class EstablishmentDetailViewModel : ViewModelBase
    {
        public Establishment _establishment { get; set; }
        public Establishment Establishment
        {
            get { return _establishment; }
            set { _establishment = value; RaisePropertyChanged(nameof(Establishment)); }
        }

        public EstablishmentDetailViewModel(Establishment establishment)
        {
            Establishment = establishment;
        }
    }
}
