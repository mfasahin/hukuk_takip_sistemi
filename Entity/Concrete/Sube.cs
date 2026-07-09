using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
    public class Sube
    {
        [Key]
        public int SUBE_ID { get; set; } //PK

        [Required]
        [StringLength(5)]
        public string SUBE_KODU { get; set; } // NOT NULL

        [Required]
        [StringLength(25)]
        public string SUBE_ADI { get; set; } // NOT NULL

        
        [StringLength(25)]
        public string SUBE_BOLGE { get; set; } 

        
        [StringLength(75)]
        public string SUBE_ADRES { get; set; } 

  
        [StringLength(15)]
        public string SUBE_TEL_NO { get; set; }


        public DateTime? GRS_TAR_ZMN { get; set; }
        public int? GRS_KULLANICI_ID { get; set; }

        public DateTime? GNC_TAR_ZMN { get; set; }
        public int? GNC_KULLANICI_ID { get; set; }

        public DateTime? SIL_TAR_ZMN { get; set; }
        public int? SIL_KULLANICI_ID { get; set; }
    }
}
