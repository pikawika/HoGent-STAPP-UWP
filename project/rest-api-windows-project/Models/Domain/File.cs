using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stappBackend.Models.Domain
{
    public class File
    {
        [Key]
        public int FileId { get; private set; }
        public String Name { get; set; }
        public string Path { get; set; }
    }
}
