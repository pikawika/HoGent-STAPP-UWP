using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stappBackend.Models.Domain
{
    public class Image
    {
        [Key]
        public int ImageId { get; private set; }
        public string Path { get; set; }
    }
}
