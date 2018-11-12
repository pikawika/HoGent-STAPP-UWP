using System;
using System.ComponentModel.DataAnnotations;

namespace stappBackend.Models
{
    public class EstablishmentSocialMedia
    {
        [Key]
        public int EstablishmentSocialMediaId { get; private set; }
        public SocialMedia SocialMediaInfo { get; set; }
        public String url { get; set; }
    }
}
