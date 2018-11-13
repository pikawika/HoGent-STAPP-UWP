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
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }

        [JsonIgnore]
        public Login Login { get; set; } = new Login();
    }
}
