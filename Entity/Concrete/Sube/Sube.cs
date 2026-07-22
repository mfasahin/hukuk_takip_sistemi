using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Concrete
{
    [Table("SUBE", Schema = "dbo")]
    public class Sube : BaseEntity
    {
        [Key]
        public Guid SUBE_ID { get; set; } //PK

        [StringLength(5)]
        public string SUBE_KODU { get; set; } 

        [StringLength(25)]
        public string SUBE_ADI { get; set; } 

        [StringLength(25)]
        public string SUBE_BOLGE { get; set; }

        [StringLength(75)]
        public string SUBE_ADRES { get; set; }

        [StringLength(15)]
        public string SUBE_TEL_NO { get; set; }

        public virtual ICollection<Ihtar> Ihtars { get; set; }
    }
}
