using System;

namespace Entity.Dto
{
    public class UrunDto
    {
        public Guid UrunId { get; set; }
        public string UrunAd { get; set; }
        public string UrunKod { get; set; }
        public DateTime SonGecerlilikTar { get; set; }
    }
}