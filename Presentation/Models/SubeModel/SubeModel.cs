using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class SubeModel
    {
        public int SubeId { get; set; } //PK

        public string SubeKodu { get; set; } // NOT NULL

        public string SubeAdı { get; set; } // NOT NULL

        public string SubeBolge { get; set; }

        public string SubeAdres { get; set; }

        public string SubeTelNo { get; set; }
    }
}