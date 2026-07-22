using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Concrete
{
    [Table("ICRA_MAHKEME", Schema = "dbo")]
    public class IcraMahkeme : BaseEntity
    {
        [Key]
        public Guid MAHKEME_ID { get; set; } // PK

        [StringLength(100)]
        public string MAHKEME_AD { get; set; }

        // Navigation Property: Bir mahkemenin birden fazla icra kaydı olabilir
        public ICollection<Icra> Icralar { get; set; }
    }
}