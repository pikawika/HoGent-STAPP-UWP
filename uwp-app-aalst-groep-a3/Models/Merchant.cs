using System.Collections.Generic;

namespace uwp_app_aalst_groep_a3.Models
{
    public class Merchant : User
    {
        public List<Company> Companies { get; set; } = new List<Company>();
    }
}
