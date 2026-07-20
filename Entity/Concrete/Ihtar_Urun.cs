using Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Concrete
{
    [Table("IHTAR_URUN", Schema = "dbo")]
    public class IhtarUrun : BaseEntity
    {
        [Key]
        public Guid IHTAR_URUN_ID { get; set; } // PK

        [Required]
        public Guid URUN_ID { get; set; } // FK

        [Required]
        public Guid IHTAR_ID { get; set; } // FK

        // Navigation Properties
        public virtual Ihtar Ihtar { get; set; }
        public virtual Urun Urun { get; set; }
    }
}