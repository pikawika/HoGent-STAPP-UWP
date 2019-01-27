using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uwp_app_aalst_groep_a3.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; private set; }
        public String Name { get; set; }
    }
}
