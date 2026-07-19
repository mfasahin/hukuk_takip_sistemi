using System;

namespace Entity.Dto
{
    public class IcraDto
    {
        public Guid IcraId { get; set; }

        public Guid IhtarUrunId { get; set; }

        public Guid MahkemeId { get; set; }
        public string MahkemeAd { get; set; }

        public DateTime IcraTakipTar { get; set; }
        public string IcraDosyaNo { get; set; }

        // IhtarUrun -> Ihtar/Urun zinciri üzerinden gelen görüntüleme alanları
        public string UrunAd { get; set; }
        public DateTime IhtarTarih { get; set; }
        public decimal BorcTutar { get; set; }

        public string MusteriAd { get; set; }
        public string AvukatAd { get; set; }
        public string SubeAd { get; set; }

        public DateTime? SilTarZmn { get; set; }
    }
}