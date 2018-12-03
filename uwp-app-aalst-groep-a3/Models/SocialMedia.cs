using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using uwp_app_aalst_groep_a3.Network;

namespace uwp_app_aalst_groep_a3.Models
{
    public class SocialMedia
    {
        public int SocialMediaId { get; private set; }
        public string Name { get; set; }

        private string logoPath;

        public string LogoPath {
            get { return NetworkAPI.baseUrl + logoPath; }
            set { logoPath = value; }
        }
        public List<EstablishmentSocialMedia> EstablishmentSocialMedias { get; set; }
    }
}
