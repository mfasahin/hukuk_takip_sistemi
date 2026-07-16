using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class IcraMahkemeModel
    {
        public int MahkemeId { get; set; } // PK
        
        public string MahkemeAd { get; set; } // NOT NULL
    }
}