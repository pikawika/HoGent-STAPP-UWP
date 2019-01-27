﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using stappBackend.Models.ViewModels.Attachments;

namespace stappBackend.Models.ViewModels.Promotion
{
    public class ModifyPromotionViewModel
    {
        public string Name { get; set; }

        public string Message { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public List<FileViewModel> Images { get; set; }

        public List<FileViewModel> Attachments { get; set; }
    }
}
