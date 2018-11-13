using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace stappBackend.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; private set; }
        public String Name { get; set; }
        [JsonIgnore]
        public List<EstablishmentCategory> EstablishmentCategories { get; set; }
    }
}
