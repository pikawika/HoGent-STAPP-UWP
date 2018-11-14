using System.Collections.Generic;

namespace uwp_app_aalst_groep_a3.Models
{
    public class Customer : User
    {
        public List<EstablishmentSubscription> EstablishmentSubscriptions { get; set; } = new List<EstablishmentSubscription>();
    }
}
