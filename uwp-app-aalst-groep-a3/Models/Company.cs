using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace uwp_app_aalst_groep_a3.Models
{
    public class Company
    {
        public int CompanyId { get; private set; }
        public string Name { get; set; }
        public List<Establishment> Establishments { get; set; } = new List<Establishment>();
    }
}
