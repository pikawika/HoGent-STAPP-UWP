using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stappBackend.Models.Domain
{
    public class Login
    {
        [Key]
        public int LoginId { get; set; }
        public string Username { get; set; }
        public string Hash { get; set; }
        public byte[] Salt { get; set; }
        public Role Role { get; set; }

        public User User { get; set; }
        public int UserLoginId { get; set; }
    }
}
