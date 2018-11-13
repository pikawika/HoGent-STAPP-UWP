using System;
using System.ComponentModel.DataAnnotations;

namespace stappBackend.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; private set; }
        public String Name { get; set; }
    }
}
