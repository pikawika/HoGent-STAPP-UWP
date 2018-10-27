using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uwp_app_aalst_groep_a3.Models
{
    public class Merchant : User
    {
        public CategoryEnum Category { get; set; }
        public List<String> Tags { get; set; }
        public List<Company> Companies { get; set; }
        public Dictionary<SocialMediaEnum, String> SocialMedia { get; set; }

        public Merchant()
        {
            SocialMedia = new Dictionary<SocialMediaEnum, String>
            {
                {SocialMediaEnum.FACEBOOK, "https://facebook.com/mycompany"},
                {SocialMediaEnum.TWITTER, "https://twitter.com/mycompany"},
                {SocialMediaEnum.INSTAGRAM, "https://instagram.com/mycompany"},
                {SocialMediaEnum.YOUTUBE, "https://youtube.com/mycompany"},
                {SocialMediaEnum.LINKEDIN, "https://linkedin.com/mycompany"},
            };
        }
    }
}
