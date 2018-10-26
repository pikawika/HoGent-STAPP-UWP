using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uwp_app_aalst_groep_a3.Models
{
    abstract class User
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
    }
}
