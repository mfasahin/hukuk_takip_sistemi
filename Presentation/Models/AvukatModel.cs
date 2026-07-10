using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class AvukatModel
    {
        public int AVUKAT_ID { get; set; } //PK

        public string AVKT_AD { get; set; } // NOT NULL

        public string AVKT_SOYAD { get; set; } // NOT NULL

        public string TBB_SICIL_NO { get; set; } // NOT NULL

        public string AVKT_EPOSTA { get; set; } // NOT NULL

        public string AVKT_TEL_NO { get; set; } // NOT NULL

        public string HKK_BURO_AD { get; set; }

        public string HKK_BURO_ADRES { get; set; }

        public string OFIS_TEL_NO { get; set; }
    }
}