﻿using System.ComponentModel.DataAnnotations;

namespace stappBackend.Models
{
    public class EstablishmentCategory
    {
        public int EstablishmentId { get; set; }
        public Establishment Establishment { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
