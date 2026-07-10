using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
    [Table("ICRA", Schema = "dbo")]
    public class Icra
    {
        [Key]
        public int ICRA_ID { get; set; } // PK

        [Required]
        public int IHTAR_URUN_ID { get; set; } // FK

        [Required]
        public int MAHKEME_ID { get; set; } // FK

        [Required]
        public DateTime ICRA_TAKIP_TAR { get; set; } // NOT NULL araştır. zaman da eklenebilir

        [Required]
        [StringLength(15)]
        public string ICRA_DOSYA_NO { get; set; } // NOT NULL

        public DateTime GRS_TAR_ZMN { get; set; } 
        public int GRS_KULLANICI_ID { get; set; } 

        public DateTime? GNC_TAR_ZMN { get; set; }
        public int? GNC_KULLANICI_ID { get; set; }

        public DateTime? SIL_TAR_ZMN { get; set; }
        public int? SIL_KULLANICI_ID { get; set; }

        // Navigation Properties
        public IhtarUrun IhtarUrun { get; set; }
        public Mahkeme Mahkeme { get; set; }
    }
}
