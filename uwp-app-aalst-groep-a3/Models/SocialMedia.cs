using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace uwp_app_aalst_groep_a3.Models
{
    public class SocialMedia
    {
        public int SocialMediaId { get; private set; }
        public string Name { get; set; }
        public string LogoPath { get; set; }
        public List<EstablishmentSocialMedia> EstablishmentSocialMedias { get; set; }
    }
}
