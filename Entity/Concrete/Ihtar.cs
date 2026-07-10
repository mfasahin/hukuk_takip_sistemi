using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Concrete
{
    [Table("IHTAR", Schema = "dbo")]
    public class Ihtar
    {
        [Key]
        public int IHTAR_ID { get; set; } // PK

        [Required]
        public int MUSTERI_ID { get; set; } // FK

        [Required]
        public int AVUKAT_ID { get; set; } // FK

        [Required]
        public int SUBE_ID { get; set; } // FK

        [Required]
        [DataType(DataType.Currency)]
        public decimal BORC_TUTAR { get; set; } // DECIMAL(12,2)

        [Required]
        public DateTime IHTAR_TAR_ZMN { get; set; } // NOT NULL

        public DateTime GRS_TAR_ZMN { get; set; }
        public int GRS_KULLANICI_ID { get; set; } 

        public DateTime? GNC_TAR_ZMN { get; set; }
        public int? GNC_KULLANICI_ID { get; set; }

        public DateTime? SIL_TAR_ZMN { get; set; }
        public int? SIL_KULLANICI_ID { get; set; }

        // Navigation Properties
        public Musteri Musteri { get; set; }
        public Avukat Avukat { get; set; }
        public Sube Sube { get; set; }
    }
}
