using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Concrete
{
    [Table("IHTAR", Schema = "dbo")]
    public class Ihtar : BaseEntity
    {
        [Key]
        public Guid IHTAR_ID { get; set; } // PK

        [Required]
        public Guid MUSTERI_ID { get; set; } // FK

        [Required]
        public Guid AVUKAT_ID { get; set; } // FK

        [Required]
        public Guid SUBE_ID { get; set; } // FK

        [DataType(DataType.Currency)]
        public decimal BORC_TUTAR { get; set; } // DECIMAL(12,2)

        public DateTime IHTAR_TAR_ZMN { get; set; }

        // Navigation Properties
        public virtual Musteri Musteri { get; set; }
        public virtual Avukat Avukat { get; set; }
        public virtual Sube Sube { get; set; }
        public virtual ICollection<IhtarUrun> IhtarUrunler { get; set; }
    }
}