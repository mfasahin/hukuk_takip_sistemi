using System;

namespace Presentation.Models
{
    public class AvukatModel : BaseModel
    {
        public Guid AvukatId { get; set; } //PK

        public string AvktAd { get; set; } // NOT NULL

        public string AvktSoyad { get; set; } // NOT NULL

        public string TbbSicilNo { get; set; } // NOT NULL

        public string AvktEposta { get; set; } // NOT NULL

        public string AvktTelNo { get; set; } // NOT NULL

        public string HkkBuroAd { get; set; }

        public string HkkBuroAdres { get; set; }

        public string OfisTelNo { get; set; }
    }
}