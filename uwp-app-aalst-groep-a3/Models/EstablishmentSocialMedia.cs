namespace uwp_app_aalst_groep_a3.Models
{
    public class EstablishmentSocialMedia
    {
        public int EstablishmentId { get; set; }
        public Establishment Establishment { get; set; }
        public int SocialMediaId { get; set; }
        public SocialMedia SocialMedia { get; set; }
        public string Url { get; set; }
    }
}
