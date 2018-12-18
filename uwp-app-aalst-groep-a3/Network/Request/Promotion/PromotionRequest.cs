using System;
using System.Collections.Generic;
using uwp_app_aalst_groep_a3.Network.Request.Attachments;

namespace uwp_app_aalst_groep_a3.Network.Request.Promotion
{
    public class PromotionRequest
    {
        public int EstablishmentId { get; set; }

        public string Name { get; set; }

        public string Message { get; set; }

        public DateTimeOffset? StartDate { get; set; }

        public DateTimeOffset? EndDate { get; set; }

        public List<FileRequest> Images { get; set; } = new List<FileRequest>();

        public List<FileRequest> Attachments { get; set; } = new List<FileRequest>();
    }
}
