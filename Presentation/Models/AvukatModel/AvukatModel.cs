using System;

namespace Presentation.Models
{
    public class AvukatModel : BaseModel
    {
        public Guid AvukatId { get; set; } //PK

        public string AvktAd { get; set; } 

        public string AvktSoyad { get; set; } 

        public string TbbSicilNo { get; set; } 

        public string AvktEposta { get; set; } 

        public string AvktTelNo { get; set; } 

        public string HkkBuroAd { get; set; }

        public string HkkBuroAdres { get; set; }

        public string OfisTelNo { get; set; }
    }
}