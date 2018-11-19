using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Network;

namespace uwp_app_aalst_groep_a3.Models.Domain
{
    public class Image
    {
        public int ImageId { get; private set; }

        private string path;

        public string Path
        {
            get { return NetworkAPI.baseUrl + path; }
            set { path = value; }
        }

    }
}
