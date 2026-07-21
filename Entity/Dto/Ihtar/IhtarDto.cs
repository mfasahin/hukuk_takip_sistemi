using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entity.Dto
{
    public class IhtarDto
    {
        public Guid IhtarId { get; set; }
        public decimal BorcTutar { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime IhtarTarih { get; set; }

        public Guid MusteriId { get; set; }
        public string MusteriNo { get; set; }
        public string MusteriAd { get; set; }
        public string MusteriSoyad { get; set; }
        
        public Guid UrunId { get; set; }
        public string UrunAd { get; set; }

        public Guid SubeId { get; set; }
        public string SubeAd { get; set; }
        public string SubeKod { get; set; }

        public Guid AvukatId { get; set; }
        public string AvukatAd { get; set; }
        public string AvukatSoyad { get; set; }

        public DateTime? SilTarZmn { get; set; }
        public List<UrunDto> Urunler { get; set; }
        public List<Guid> SecilenUrunler { get; set; } = new List<Guid>();


    }
}