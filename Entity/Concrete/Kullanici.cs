using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
    [Table("KULLANICI", Schema = "dbo")]
    public class Kullanici
    {
        [Key]
        public int KULLANICI_ID { get; set; } //PK

        [Required]
        [StringLength(25)]
        public string KULLANICI_AD { get; set; } // NOT NULL

        [Required]
        [StringLength(25)]
        public string SIFRE { get; set; } // NOT NULL

        public DateTime? GRS_TAR_ZMN { get; set; }
        public int? GRS_KULLANICI_ID { get; set; }

        public DateTime? GNC_TAR_ZMN { get; set; }
        public int? GNC_KULLANICI_ID { get; set; }

        public DateTime? SIL_TAR_ZMN { get; set; }
        public int? SIL_KULLANICI_ID { get; set; }
    }
}
