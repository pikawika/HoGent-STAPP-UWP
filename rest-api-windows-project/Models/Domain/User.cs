using System;
using System.ComponentModel.DataAnnotations;

namespace stappBackend.Models
{
    public abstract class User
    {
        [Key]
        public String UserId { get; private set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
    }
}
