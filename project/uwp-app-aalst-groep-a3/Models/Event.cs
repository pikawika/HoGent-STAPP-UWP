using System;
using System.Collections.Generic;
using uwp_app_aalst_groep_a3.Models.Domain;

namespace uwp_app_aalst_groep_a3.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        public List<Image> Images { get; set; } = new List<Image>();
        public List<File> Attachments { get; set; } = new List<File>();

        public bool isDeleted { get; set; } = false;

        public Establishment Establishment { get; set; }
    }
}
