using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uwp_app_aalst_groep_a3.Interfaces
{
    public interface ISocialMedia
    {
        int ISocialMediaId { get; set; }
        String Name { get; set; }
        String Image { get; set; }
    }
}
