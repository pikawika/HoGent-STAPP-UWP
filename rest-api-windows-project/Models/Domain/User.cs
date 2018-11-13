using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using stappBackend.Models.Domain;

namespace stappBackend.Models
{
    public abstract class User
    {
        [Key]
        public int UserId { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public Login Login { get; set; } = new Login();
    }
}
