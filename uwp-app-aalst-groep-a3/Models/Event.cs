using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using uwp_app_aalst_groep_a3.Models.Domain;

namespace uwp_app_aalst_groep_a3.Models
{
    public class Event
    {
        public int EventId { get; private set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public List<Image> Images { get; set; } = new List<Image>();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Establishment Establishment { get; set; }
    }
}
