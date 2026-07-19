namespace Entity.Dto
{
    public class IhtarUrunDto
    {
        public int IhtarUrunId { get; set; }
        public int IhtarId { get; set; }
        public int UrunId { get; set; }
        public string UrunAd { get; set; }   // UrunDto'yu tekrar yazmak yerine sadece ihtiyaç duyulan alan
        public string DisplayText { get; set; }
    }
}