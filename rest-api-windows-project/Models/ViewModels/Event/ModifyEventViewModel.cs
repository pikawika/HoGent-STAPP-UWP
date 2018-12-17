using System;
using System.Collections.Generic;
using stappBackend.Models.ViewModels.Attachments;

namespace stappBackend.Models.ViewModels.Event
{
    public class ModifyEventViewModel
    {
        public string Name { get; set; }

        public string Message { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public List<FileViewModel> Images { get; set; }

        public List<FileViewModel> Attachments { get; set; }
    }
}
