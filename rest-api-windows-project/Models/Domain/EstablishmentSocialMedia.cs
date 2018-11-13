using System;
using System.ComponentModel.DataAnnotations;

namespace stappBackend.Models
{
    public class EstablishmentSocialMedia
    {
        public int EstablishmentId { get; set; }
        public Establishment Establishment { get; set; }
        public int SocialMediaId { get; set; }
        public SocialMedia SocialMedia { get; set; }
        public String url { get; set; }
    }
}
