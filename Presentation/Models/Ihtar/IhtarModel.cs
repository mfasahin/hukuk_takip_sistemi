using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class IhtarModel
    {
        public int IHTAR_ID { get; set; } // PK

        public int MUSTERI_ID { get; set; } // FK

        public int AVUKAT_ID { get; set; } // FK

        public int SUBE_ID { get; set; } // FK

        public decimal BORC_TUTAR { get; set; } // DECIMAL(12,2)

        public DateTime IHTAR_TAR_ZMN { get; set; } // NOT NULL
    }
}