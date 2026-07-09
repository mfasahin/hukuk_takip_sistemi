using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
    public class Mahkeme
    {
        [Key]
        public int MAHKEME_ID { get; set; } // PK

        [Required]
        [StringLength(100)]
        public string MAHKEME_AD { get; set; } // NOT NULL

        
        public DateTime GRS_TAR_ZMN { get; set; } 
        public int GRS_KULLANICI_ID { get; set; } 

        public DateTime? GUNCELLE_TAR_ZMN { get; set; }
        public int? GNC_KULLANICI_ID { get; set; }

        public DateTime? SIL_TAR_ZMN { get; set; }
        public int? SIL_KULLANICI_ID { get; set; }

        // Navigation Property: Bir mahkemenin birden fazla icra kaydı olabilir
        public ICollection<Icra> Icralar { get; set; }
    }
}
