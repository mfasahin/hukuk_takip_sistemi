using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
    [Table("IHTAR_URUN", Schema = "dbo")]
    public class IhtarUrun
    {
        [Key]
        public int IHTAR_URUN_ID { get; set; } // PK

        [Required]
        public int URUN_ID { get; set; } // FK

        [Required]
        public int IHTAR_ID { get; set; } // FK
        public DateTime? GRS_TAR_ZMN { get; set; }
        public int? GRS_KULLANICI_ID { get; set; }

        public DateTime? GNC_TAR_ZMN { get; set; }
        public int? GNC_KULLANICI_ID { get; set; }

        public DateTime? SIL_TAR_ZMN { get; set; }
        public int? SIL_KULLANICI_ID { get; set; }

        // Navigation Properties
        public virtual Ihtar Ihtar { get; set; }
        public virtual Urun Urun { get; set; }
    }
}
