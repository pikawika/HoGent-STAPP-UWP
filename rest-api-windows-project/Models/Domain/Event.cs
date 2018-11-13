using System;
using System.ComponentModel.DataAnnotations;

namespace stappBackend.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; private set; }
        public String Name { get; set; }
    }
}
