using System;
using System.ComponentModel.DataAnnotations;

namespace stappBackend.Models
{
    public class EstablishmentSubscription
    {
        [Key]
        public int EstablishmentSubscriptionId { get; private set; }
        public Establishment Establishment { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
