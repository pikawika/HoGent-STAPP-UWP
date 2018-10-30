using System.Collections.Generic;

namespace stappBackend.Models
{
    public class Merchant : User
    {
        public List<Company> Companies { get; set; }
    }
}
