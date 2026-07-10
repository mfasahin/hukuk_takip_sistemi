using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
    [Table("MUSTERI", Schema = "dbo")]
    public class Musteri
    {
        [Key] 
        public int MUSTERI_ID { get; set; }   // PK

        [Required]
        [StringLength(8)]
        public string MUST_NO { get; set; }   // NOT NULL

        [Required]
        [StringLength(25)]
        public string MUST_AD { get; set; }   // NOT NULL

        [StringLength(25)]
        public string MUST_SOYAD { get; set; }

        [StringLength(11)]
        public string MUST_KIMLIK_NO { get; set; }

        [StringLength(10)]
        public string MUST_VKN_NO { get; set; } //düzeltildi

        [StringLength(50)]
        public string MUST_EPOSTA { get; set; }

        [StringLength(15)]
        public string MUST_TEL_NO { get; set; }

        public DateTime? GRS_TAR_ZMN { get; set; }
        public int? GRS_KULLANICI_ID { get; set; }

        public DateTime? GNC_TAR_ZMN { get; set; }
        public int? GNC_KULLANICI_ID { get; set; }

        public DateTime? SIL_TAR_ZMN { get; set; }
        public int? SIL_KULLANICI_ID { get; set; }
    }
}
