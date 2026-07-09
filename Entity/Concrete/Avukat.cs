using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
    public class Avukat
    {
        [Key]
        public int AVUKAT_ID { get; set; } //PK

        [Required]
        [StringLength(25)]
        public string AVKT_AD { get; set; } // NOT NULL

        [Required]
        [StringLength(25)]
        public string AVKT_SOYAD { get; set; } // NOT NULL

        [Required]
        [StringLength(10)]
        public string TBB_SICIL_NO { get; set; } // NOT NULL

        [Required]
        [StringLength(25)]
        public string AVKT_EPOSTA { get; set; } // NOT NULL

        [Required]
        [StringLength(15)]
        public string AVKT_TEL_NO { get; set; } // NOT NULL

        [StringLength(50)]
        public string HKK_BURO_AD { get; set; }

        [StringLength(70)]
        public string HKK_BURO_ADRES { get; set; }

        [StringLength(15)]
        public string OFIS_TEL_NO { get; set; }

        public DateTime? GRS_TAR_ZMN { get; set; }
        public int? GRS_KULLANICI_ID { get; set; }

        public DateTime? GNC_TAR_ZMN { get; set; }
        public int? GNC_KULLANICI_ID { get; set; }

        public DateTime? SIL_TAR_ZMN { get; set; }
        public int? SIL_KULLANICI_ID { get; set; }
    }
}
