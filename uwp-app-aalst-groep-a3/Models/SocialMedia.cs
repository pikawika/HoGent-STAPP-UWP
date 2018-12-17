using System.Collections.Generic;

namespace uwp_app_aalst_groep_a3.Models
{
    public class SocialMedia
    {
        public int SocialMediaId { get; set; }
        public string Name { get; set; }
        public string LogoPath { get; set; }
        public List<EstablishmentSocialMedia> EstablishmentSocialMedias { get; set; } = new List<EstablishmentSocialMedia>();
    }
}
