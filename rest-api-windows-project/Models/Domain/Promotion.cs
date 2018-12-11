using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using stappBackend.Models.Domain;

namespace stappBackend.Models
{
    public class Promotion
    {
        [Key]
        public int PromotionId { get; private set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public bool isDeleted { get; set; } = false;
        public List<Image> Images { get; set; } = new List<Image>();
        public List<File> Attachments { get; set; } = new List<File>();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Establishment Establishment { get; set; }
    }
}
