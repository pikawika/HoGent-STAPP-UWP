using uwp_app_aalst_groep_a3.Models.Domain;

namespace uwp_app_aalst_groep_a3.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public Login Login { get; set; } = new Login();
    }
}
