using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace stappBackend.Models
{
    public class SocialMedia
    {
        [Key]
        public int SocialMediaId { get; private set; }
        public string Name { get; set; }
        public string LogoPath { get; set; }
        [JsonIgnore]
        public List<EstablishmentSocialMedia> EstablishmentSocialMedias { get; set; }
    }
}
