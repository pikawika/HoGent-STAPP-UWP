using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uwp_app_aalst_groep_a3.Models.Domain
{
    public class ExceptionalDay
    {
        [Key]
        public int ExceptionalDayId { get; set; }
        public DateTime Day { get; set; }
        public String Message { get; set; }
    }
}
