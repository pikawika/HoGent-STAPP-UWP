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

        public string Name { get; set; }
        public string Description { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public List<EstablishmentCategory> EstablishmentCategories { get; set; } = new List<EstablishmentCategory>();
        public List<EstablishmentSocialMedia> EstablishmentSocialMedias { get; set; } = new List<EstablishmentSocialMedia>();
        public List<Image> Images { get; set; } = new List<Image>();

        public List<OpenDay> OpenDays { get; set; } = new List<OpenDay>();
        public List<ExceptionalDay> ExceptionalDays { get; set; } = new List<ExceptionalDay>();

        public List<Promotion> Promotions { get; set; } = new List<Promotion>();
        public List<Event> Events { get; set; } = new List<Event>();

        [JsonIgnore]
        public List<EstablishmentSubscription> EstablishmentSubscriptions { get; set; }
    }
}
