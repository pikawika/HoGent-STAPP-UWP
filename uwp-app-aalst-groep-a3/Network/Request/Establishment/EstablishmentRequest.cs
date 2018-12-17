using System.Collections.Generic;
using uwp_app_aalst_groep_a3.Network.Request.Attachments;
using uwp_app_aalst_groep_a3.Network.Request.Establishment;

namespace uwp_app_aalst_groep_a3.Network.requests
{
    public class EstablishmentRequest
    {
        public int CompanyId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public List<CategoryRequest> Categories { get; set; } = new List<CategoryRequest>();

        public List<OpenDayRequest> OpenDays { get; set; } = new List<OpenDayRequest>();

        public List<SocialMediaRequest> SocialMedias { get; set; } = new List<SocialMediaRequest>();

        public List<ExceptionalDayRequest> ExceptionalDays { get; set; } = new List<ExceptionalDayRequest>();

        public List<FileRequest> Images { get; set; } = new List<FileRequest>();
    }
}
