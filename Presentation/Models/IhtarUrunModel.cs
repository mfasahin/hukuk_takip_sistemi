using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class IhtarUrunModel
    {
        public int IHTAR_URUN_ID { get; set; } // PK

        public int URUN_ID { get; set; } // FK

        public int IHTAR_ID { get; set; } // FK
    }
}