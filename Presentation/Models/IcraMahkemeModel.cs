using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class IcraMahkemeModel
    {
        public int MAHKEME_ID { get; set; } // PK
        
        public string MAHKEME_AD { get; set; } // NOT NULL
    }
}