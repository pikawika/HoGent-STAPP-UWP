using System;
using System.ComponentModel.DataAnnotations;

namespace stappBackend.Models
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
