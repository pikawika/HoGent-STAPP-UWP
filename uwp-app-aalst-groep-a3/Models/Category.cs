using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace uwp_app_aalst_groep_a3.Models
{
    public class Category
    {
        public int CategoryId { get; private set; }
        public string Name { get; set; }
        public List<EstablishmentCategory> EstablishmentCategories { get; set; }
    }
}
