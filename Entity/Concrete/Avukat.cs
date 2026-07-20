using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Concrete
{
    [Table("AVUKAT", Schema = "dbo")]
    public class Avukat : BaseEntity
    {

        [Key]
        public Guid AVUKAT_ID { get; set; } //PK

        [StringLength(25)]
        public string AVKT_AD { get; set; }

        [StringLength(25)]
        public string AVKT_SOYAD { get; set; }

        [StringLength(10)]
        public string TBB_SICIL_NO { get; set; }

        [StringLength(25)]
        public string AVKT_EPOSTA { get; set; }

        [StringLength(15)]
        public string AVKT_TEL_NO { get; set; }

        [StringLength(50)]
        public string HKK_BURO_AD { get; set; }

        [StringLength(70)]
        public string HKK_BURO_ADRES { get; set; }

        [StringLength(15)]
        public string OFIS_TEL_NO { get; set; }

        public virtual ICollection<Ihtar> Ihtars { get; set; }
    }
}