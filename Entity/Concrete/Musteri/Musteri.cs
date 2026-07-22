using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Concrete
{
    [Table("MUSTERI", Schema = "dbo")]
    public class Musteri : BaseEntity
    {
        [Key]
        public Guid MUSTERI_ID { get; set; }  // PK

        [StringLength(8)]
        public string MUST_NO { get; set; }

        [StringLength(25)]
        public string MUST_AD { get; set; } 

        [StringLength(25)]
        public string MUST_SOYAD { get; set; }

        [StringLength(11)]
        public string MUST_KIMLIK_NO { get; set; }

        [StringLength(10)]
        public string MUST_VKN_NO { get; set; } 

        [StringLength(50)]
        public string MUST_EPOSTA { get; set; }

        [StringLength(15)]
        public string MUST_TEL_NO { get; set; }

        public virtual ICollection<Ihtar> Ihtars { get; set; }
    }
}
