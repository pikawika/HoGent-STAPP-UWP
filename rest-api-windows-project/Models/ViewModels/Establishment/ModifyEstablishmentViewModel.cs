using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using stappBackend.Models.ViewModels.Attachments;

namespace stappBackend.Models.ViewModels.Establishment
{
    public class ModifyEstablishmentViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public List<CategoryViewModel> Categories { get; set; }

        public List<SocialMediaViewModel> SocialMedias { get; set; }

        public List<OpenDayViewModel> OpenDays { get; set; }

        public List<ExceptionalDayViewModel> ExceptionalDays { get; set; }

        public List<FileViewModel> Images { get; set; }
    }
}
