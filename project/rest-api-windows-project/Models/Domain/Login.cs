using Newtonsoft.Json;
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
        [JsonIgnore]
        public int LoginId { get; set; }
        public string Username { get; set; }
        [JsonIgnore]
        public string Hash { get; set; }
        [JsonIgnore]
        public byte[] Salt { get; set; }
        [JsonIgnore]
        public Role Role { get; set; }

        [JsonIgnore]
        public User User { get; set; }
        [JsonIgnore]
        public int UserLoginId { get; set; }
    }
}
