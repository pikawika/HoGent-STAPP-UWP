using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace stappBackend.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; private set; }

        public String Name { get; set; }

        public List<Establishment> Establishments { get; set; } = new List<Establishment>();
    }
}
