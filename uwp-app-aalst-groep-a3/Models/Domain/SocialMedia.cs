using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uwp_app_aalst_groep_a3.Models.Domain
{
    public class SocialMedia
    {
        [Key]
        public int SocialMediaId { get; private set; }
        public String Name { get; set; }
        public String LogoPath { get; set; }
    }
}
