using System.Collections.Generic;

namespace uwp_app_aalst_groep_a3.Models
{
    public class Company
    {
        public int CompanyId { get; set; }

        public string Name { get; set; }

        public bool isDeleted { get; set; } = false;

        public List<Establishment> Establishments { get; set; } = new List<Establishment>();
    }
}
