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

        public List<EstablishmentCategory> Categories { get; set; }
        public List<Promotion> Promotions { get; set; }
        public List<Event> Events { get; set; }
        public List<EstablishmentSocialMedia> Socials { get; set; }

        public List<ExceptionalDay> ExceptionalDays { get; set; }
        public List<OpenDay> OpenDays { get; set; }
    }
}
