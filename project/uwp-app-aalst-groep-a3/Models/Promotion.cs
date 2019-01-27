using System;
using System.Collections.Generic;
using uwp_app_aalst_groep_a3.Models.Domain;

namespace uwp_app_aalst_groep_a3.Models
{
    public class Promotion
    {
        public int PromotionId { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public bool isDeleted { get; set; } = false;
        public List<Image> Images { get; set; } = new List<Image>();
        public List<File> Attachments { get; set; } = new List<File>();
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        public Establishment Establishment { get; set; }
    }
}
