using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Concrete
{
    [Table("URUN", Schema = "dbo")]
    public class Urun : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid URUN_ID { get; set; } //PK

        [StringLength(25)]
        public string URUN_AD { get; set; }

        [StringLength(5)]
        public string URUN_KOD { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? SON_GECERLILIK_TAR { get; set; }

        public virtual ICollection<IhtarUrun> IhtarUrunler { get; set; }
    }
}