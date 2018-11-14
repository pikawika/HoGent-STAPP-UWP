using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace uwp_app_aalst_groep_a3.Models.Domain
{
    public class Image
    {
        public int ImageId { get; private set; }
        public string Path { get; set; }
    }
}
