using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
    public class Urun
    {
        [Key]
        public int URUN_ID { get; set; } //PK

        [Required]
        [StringLength(25)]
        public string URUN_AD { get; set; } // NOT NULL

        [Required]
        [StringLength(5)]
        public string URUN_KOD { get; set; } // NOT NULL

        public DateTime? SON_GECERLILIK_TAR { get; set; } 



        public DateTime? GRS_TAR_ZMN { get; set; }
        public int? GRS_KULLANICI_ID { get; set; }

        public DateTime? GNC_TAR_ZMN { get; set; }
        public int? GNC_KULLANICI_ID { get; set; }

        public DateTime? SIL_TAR_ZMN { get; set; }
        public int? SIL_KULLANICI_ID { get; set; }
    }
}
