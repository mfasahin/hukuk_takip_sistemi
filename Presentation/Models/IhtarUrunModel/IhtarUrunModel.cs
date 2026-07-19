using System;

namespace Presentation.Models
{
    public class IhtarUrunModel
    {
        public Guid IhtarUrunId { get; set; } // PK

        public Guid UrunId { get; set; } // FK
        public UrunModel Urun { get; set; }

        public Guid IhtarId { get; set; } // FK

        public IhtarModel Ihtar { get; set; }
    }
}