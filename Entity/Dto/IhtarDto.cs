using System;
using System.Collections.Generic;

namespace Entity.Dto
{
    public class IhtarDto
    {
        public int IhtarId { get; set; }
        public decimal BorcTutar { get; set; }
        public DateTime IhtarTarih { get; set; }

        public int MusteriId { get; set; }
        public string MusteriAd { get; set; }

        public int AvukatId { get; set; }
        public string AvukatAd { get; set; }

        public int SubeId { get; set; }
        public string SubeAd { get; set; }

        public int UrunId { get; set; }
        public string UrunAd {  get; set; }

        public DateTime? SilTarZmn { get; set; }
        public List<UrunDto> Urunler { get; set; }

    }
}