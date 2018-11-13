using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace stappBackend.Models
{
    public class Establishment
    {
        [Key]
        public int EstablishmentId { get; private set; }

        public String Name { get; set; }
        public String Postcode { get; set; }
        public String Gemeente { get; set; }
        public String Straatnaam { get; set; }
        public String Huisnummer { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }

        public List<EstablishmentCategory> Categories { get; set; } = new List<EstablishmentCategory>();
        public List<Promotion> Promotions { get; set; } = new List<Promotion>();
        public List<Event> Events { get; set; } = new List<Event>();
        public List<EstablishmentSocialMedia> Socials { get; set; } = new List<EstablishmentSocialMedia>();

        public List<ExceptionalDay> ExceptionalDays { get; set; } = new List<ExceptionalDay>();
        public List<OpenDay> OpenDays { get; set; } = new List<OpenDay>();
    }
}
