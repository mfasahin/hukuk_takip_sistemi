using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class IcraModel
    {
        public Guid IcraId { get; set; } // PK

        public Guid IhtarUrunId { get; set; } // FK

        public Guid MahkemeId { get; set; } // FK

        public DateTime IcraTakipTar { get; set; } // NOT NULL araştır.

        public string IcraDosyaNo { get; set; } // NOT NULL
    }
}