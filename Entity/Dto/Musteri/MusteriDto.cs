using System;

namespace Entity.Dto
{
    public class MusteriDto
    {
        public Guid MusteriId { get; set; }
        public string MustAd { get; set; }
        public string MustSoyad { get; set; }

        // İhtiyaç halinde kullanılabilecek birleştirilmiş isim özelliği
        public string MusteriAdSoyad => $"{MustAd} {MustSoyad}".Trim();
    }
}