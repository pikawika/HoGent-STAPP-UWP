using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uwp_app_aalst_groep_a3.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public String Name { get; set; }
        public Address Address { get; set; }
        public List<Promotion> Promotions { get; set; }
        public List<Event> Events { get; set; }
        //openingsuren
    }
}
