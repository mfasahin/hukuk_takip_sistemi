using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class IhtarModel
    {
        //public int IhtarId {  get; set; }
        //public int MusteriId { get; set; }
        //public string MusteriAd { get; set; }

        //public int SubeId { get; set; }
        //public string SubeAd { get; set; }

        //public int AvukatId { get; set; }
        //public string AvukatAd { get; set; }

        //public decimal BorcTutar { get; set; } // DECIMAL(12,2)

        //[DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime IhtarTarZmn { get; set; } 

        public DateTime? GrsTarZmn { get; set; }

        public DateTime? GncTarZmn { get; set; }

        public DateTime? SilTarZmn { get; set; }

        //public List<IhtarUrunModel> Urunler { get; set; } // <-- Burada List<IhtarUrunModel>

        //// İhtara bağlı ürünler
        ////public List<UrunModel> Urunler { get; set; }

        //// Yeni ihtar eklerken seçilen ürünlerin ID’leri
        //public List<int> SecilenUrunler { get; set; }


        public int IhtarId { get; set; }

        // FK alanları
        public int MusteriId { get; set; }
        public int SubeId { get; set; }
        public int AvukatId { get; set; }

        // Görüntülenecek alanlar
        public string MusteriAd { get; set; }
        public string SubeAd { get; set; }
        public string AvukatAd { get; set; }

        public decimal BorcTutar { get; set; }
        public DateTime IhtarTarZmn { get; set; }

        // Ürünler
        public List<UrunModel> Urunler { get; set; }

        // Seçilen ürünler (ekleme/güncelleme için)
        public List<int> SecilenUrunler { get; set; }

    }
}