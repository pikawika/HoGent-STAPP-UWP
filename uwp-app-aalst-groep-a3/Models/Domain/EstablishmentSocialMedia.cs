using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uwp_app_aalst_groep_a3.Models.Domain
{
    public class EstablishmentSocialMedia
    {
        [Key]
        public int EstablishmentSocialMediaId { get; private set; }
        SocialMedia SocialMediaInfo { get; set; }
        String url { get; set; }
    }
}
