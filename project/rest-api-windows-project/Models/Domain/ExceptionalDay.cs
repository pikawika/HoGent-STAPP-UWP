using System;
using System.ComponentModel.DataAnnotations;

namespace stappBackend.Models
{
    public class ExceptionalDay
    {
        [Key]
        public int ExceptionalDayId { get; set; }
        public DateTime Day { get; set; }
        public string Message { get; set; }
    }
}
