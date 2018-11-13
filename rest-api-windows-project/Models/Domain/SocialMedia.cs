using System;
using System.ComponentModel.DataAnnotations;

namespace stappBackend.Models
{
    public class SocialMedia
    {
        [Key]
        public int SocialMediaId { get; private set; }
        public String Name { get; set; }
        public String LogoPath { get; set; }
    }
}
