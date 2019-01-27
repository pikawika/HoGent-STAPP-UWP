using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Models.Domain;

namespace uwp_app_aalst_groep_a3.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; private set; }

        public String Name { get; set; }

        public List<Establishment> Establishments { get; set; }
    }
}
