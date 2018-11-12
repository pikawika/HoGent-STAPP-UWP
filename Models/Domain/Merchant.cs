using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uwp_app_aalst_groep_a3.Models
{
    public class Merchant : User
    {
        public List<Company> Companies { get; set; }
    }
}
