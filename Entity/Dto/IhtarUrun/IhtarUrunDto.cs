using System;

namespace Entity.Dto
{
    public class IhtarUrunDto
    {
        public Guid IhtarUrunId { get; set; }
        public Guid IhtarId { get; set; }
        public Guid UrunId { get; set; }
        public string UrunAd { get; set; }   // UrunDto'yu tekrar yazmak yerine sadece ihtiyaç duyulan alan
        public string DisplayText { get; set; }
    }
}