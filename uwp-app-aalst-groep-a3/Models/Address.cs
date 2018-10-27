using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uwp_app_aalst_groep_a3.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public String Street { get; set; }
        public String HouseNumber { get; set; }
        public String Bus { get; set; }
        public String City { get; set; }
        public String PostalCode { get; set; }
    }
}
