using System;
using System.ComponentModel.DataAnnotations;

namespace uwp_app_aalst_groep_a3.Models
{
    public class EstablishmentSubscription
    {
        public int UserId { get; private set; }
        public Customer Customer  { get; set; }
        public int EstablishmentId { get; set; }
        public Establishment Establishment { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
