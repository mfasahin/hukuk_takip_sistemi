using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entity.Dto
{
    public class IhtarDto
    {
        public int IhtarId { get; set; }
        public decimal BorcTutar { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
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