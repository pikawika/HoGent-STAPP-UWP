using System.Collections.Generic;

namespace uwp_app_aalst_groep_a3.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public List<EstablishmentCategory> EstablishmentCategories { get; set; }
    }
}
