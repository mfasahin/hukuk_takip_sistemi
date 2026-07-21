using Entity.Concrete;
using System;

namespace Presentation.Models
{
    public class IhtarUrunModel : BaseModel
    {
        public Guid IhtarUrunId { get; set; } // PK

        public Guid UrunId { get; set; } // FK
        public virtual Urun Urun { get; set; }

        public Guid IhtarId { get; set; } // FK

        public virtual Ihtar Ihtar { get; set; }
    }
}