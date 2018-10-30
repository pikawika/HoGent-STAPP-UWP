using System;
using System.ComponentModel.DataAnnotations;

namespace stappBackend.Models
{
    public class Promotion
    {
        [Key]
        public int PromotionId { get; private set; }
        public String Name { get; set; }
    }
}
