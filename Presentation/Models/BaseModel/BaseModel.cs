using System;

namespace Presentation.Models
{
    public abstract class BaseModel
    {
        public DateTime? GrsTarZmn { get; set; }
        public Guid? GrsKullaniciId { get; set; }

        public DateTime? GncTarZmn { get; set; }
        public Guid? GncKullaniciId { get; set; }

        public DateTime? SilTarZmn { get; set; }
        public Guid? SilKullaniciId { get; set; }
    }
}