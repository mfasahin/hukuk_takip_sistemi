using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class IhtarModel : BaseModel
    {
        public Guid IhtarId { get; set; }

        // FK alanları
        public Guid MusteriId { get; set; }
        public Guid SubeId { get; set; }
        public Guid AvukatId { get; set; }
        public Guid UrunId { get; set; }

        // Görüntülenecek alanlar
        public string MusteriAd { get; set; }
        public string SubeAd { get; set; }
        public string AvukatAd { get; set; }
        public string UrunAd { get; set; }

        public decimal BorcTutar { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime IhtarTarZmn { get; set; }

        // Ürünler
        public List<UrunModel> Urunler { get; set; }

        // Seçilen ürünler (ekleme/güncelleme için)
        public List<Guid> SecilenUrunler { get; set; }
        public virtual ICollection<IhtarUrun> IhtarUrunler { get; set; }

    }
}