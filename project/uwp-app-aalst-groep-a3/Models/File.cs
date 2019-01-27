using System;
using System.ComponentModel.DataAnnotations;

namespace uwp_app_aalst_groep_a3.Models.Domain
{
    public class File
    {
        public int FileId { get; set; }
        public String Name { get; set; }
        public string Path { get; set; }
    }
}
