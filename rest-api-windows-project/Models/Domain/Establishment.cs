using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using stappBackend.Models.Domain;

namespace stappBackend.Models
{
    public class Establishment
    {
        [Key]
        public int EstablishmentId { get; private set; }

        public String Name { get; set; }
        public String PostalCode { get; set; }
        public String City { get; set; }
        public String Street { get; set; }
        public String HouseNumber { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }

        public List<EstablishmentCategory> EstablishmentCategories { get; set; } = new List<EstablishmentCategory>();
        public List<Promotion> Promotions { get; set; } = new List<Promotion>();
        public List<Event> Events { get; set; } = new List<Event>();
        public List<EstablishmentSocialMedia> EstablishmentSocialMedias { get; set; } = new List<EstablishmentSocialMedia>();

        public List<Image> Images { get; set; } = new List<Image>();

        public List<ExceptionalDay> ExceptionalDays { get; set; } = new List<ExceptionalDay>();
        public List<OpenDay> OpenDays { get; set; } = new List<OpenDay>();

        [JsonIgnore]
        public List<EstablishmentSubscription> EstablishmentSubscriptions { get; set; }
    }
}
