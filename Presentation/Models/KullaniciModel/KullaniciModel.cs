using System;

namespace Presentation.Models
{
    public class KullaniciModel : BaseModel
    {
        public Guid KullaniciId { get; set; } //PK

        public string KullaniciAd { get; set; } // NOT NULL

        public string Sifre { get; set; } // NOT NULL
    }
}