using System;

namespace Presentation.Models
{
    public class AvukatModel
    {
        public int AvukatId { get; set; } //PK

        public string AvktAd { get; set; } // NOT NULL

        public string AvktSoyad { get; set; } // NOT NULL

        public string TbbSicilNo { get; set; } // NOT NULL

        public string AvktEposta { get; set; } // NOT NULL

        public string AvktTelNo { get; set; } // NOT NULL

        public string HkkBuroAd { get; set; }

        public string HkkBuroAdres { get; set; }

        public string OfisTelNo { get; set; }

        public DateTime? GrsTarZmn { get; set; }

        public DateTime? GncTarZmn { get; set; } 

        public DateTime? SilTarZmn { get; set; } // soft delete için
    }
}