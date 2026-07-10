using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class IcraModel
    {
        public int ICRA_ID { get; set; } // PK

        public int IHTAR_URUN_ID { get; set; } // FK

        public int MAHKEME_ID { get; set; } // FK

        public DateTime ICRA_TAKIP_TAR { get; set; } // NOT NULL araştır. zaman da eklenebilir

        public string ICRA_DOSYA_NO { get; set; } // NOT NULL
    }
}