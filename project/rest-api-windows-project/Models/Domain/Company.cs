using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace stappBackend.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; private set; }

        public string Name { get; set; }

        public bool isDeleted { get; set; } = false;

        public List<Establishment> Establishments { get; set; } = new List<Establishment>();

        [JsonIgnore]
        public Merchant Merchant { get; set; }
        [JsonIgnore]
        public int MerchantId { get; set; }
    }
}
