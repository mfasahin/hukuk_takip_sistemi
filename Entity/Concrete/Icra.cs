using Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Concrete
{
    [Table("ICRA", Schema = "dbo")]
    public class Icra : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ICRA_ID { get; set; } // PK

        [Required]
        public Guid IHTAR_URUN_ID { get; set; } // FK

        [Required]
        public Guid MAHKEME_ID { get; set; } // FK

        public DateTime ICRA_TAKIP_TAR { get; set; }

        [StringLength(15)]
        public string ICRA_DOSYA_NO { get; set; }

        // Navigation Properties
        public IhtarUrun IhtarUrun { get; set; }
        public IcraMahkeme Mahkeme { get; set; }
    }
}