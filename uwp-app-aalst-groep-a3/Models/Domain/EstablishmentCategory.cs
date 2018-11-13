using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uwp_app_aalst_groep_a3.Models.Domain
{
    public class EstablishmentCategory
    {
        [Key]
        public int EstablishmentCategoryId { get; private set; }
        public Category Category { get; set; }
    }
}
