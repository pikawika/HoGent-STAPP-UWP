using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uwp_app_aalst_groep_a3.Models
{
    public class Merchant : User
    {
        public int MerchantId { get; set; }
        public Category Category { get; set; }
        public List<String> Tags { get; set; }
        public List<Company> Companies { get; set; }
        //sociale media
    }
}
