using uwp_app_aalst_groep_a3.Network;

namespace uwp_app_aalst_groep_a3.Models.Domain
{
    public class Image
    {
        public int ImageId { get; set; }
        private string path;

        public string Path
        {
            get { return NetworkAPI.baseUrl + path; }
            set { path = value; }
        }
    }
}
