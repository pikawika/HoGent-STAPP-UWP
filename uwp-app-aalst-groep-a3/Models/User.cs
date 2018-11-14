using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using uwp_app_aalst_groep_a3.Models.Domain;

namespace uwp_app_aalst_groep_a3.Models
{
    public abstract class User
    {
        public int UserId { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public Login Login { get; set; } = new Login();
    }
}
