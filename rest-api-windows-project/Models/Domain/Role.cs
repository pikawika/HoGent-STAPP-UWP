using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stappBackend.Models.Domain
{
    public class Role
    {
        [Key]
        public string Name { get; set; }
    }
}
