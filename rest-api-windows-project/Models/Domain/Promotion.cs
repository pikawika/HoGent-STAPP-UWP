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
        public String Name { get; set; }
        public List<Image> Images { get; set; } = new List<Image>();
    }
}
