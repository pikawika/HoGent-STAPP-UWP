using System.Collections.Generic;

namespace stappBackend.Models
{
    public class Customer : User
    {
        public List<EstablishmentSubscription> EstablishmentSubscriptions { get; set; } = new List<EstablishmentSubscription>();
    }
}
