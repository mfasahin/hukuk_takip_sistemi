using System;

namespace Entity.Dto
{
    public class IcraDto
    {
        public Guid IcraId { get; set; }
        public string IcraMahkemesi { get; set; }
        public DateTime IcraTakipTar {  get; set; }
        public string IcraDosyaNo { get; set; }

        // İlişkili alanlar (müşteri, ürün, ihtar, avukat)
        public Guid MusteriId { get; set; }
        public string MusteriNo { get; set; }
        public string MusteriAd { get; set; }
        public string MusteriSoyad { get; set; }

        public Guid UrunId { get; set; }
        public string UrunAd { get; set; }

        public decimal BorcTutar { get; set; }
        public DateTime IhtarTarih { get; set; }

        public Guid AvukatId { get; set; }
        public string AvukatAd { get; set; }
        public string AvukatSoyad { get; set; }

        public Guid IhtarUrunId { get; set; }
        public Guid MahkemeId { get; set; }
        public string MahkemeAd { get; set; }


        public DateTime? SilTarZmn { get; set; }

        //List<IhtarUrunDto>
    }

}