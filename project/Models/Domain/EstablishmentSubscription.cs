using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uwp_app_aalst_groep_a3.Models.Domain
{
    public class EstablishmentSubscription
    {
        [Key]
        public int EstablishmentSubscriptionId { get; private set; }
        public Establishment Establishment { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
