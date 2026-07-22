using Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Concrete
{
    [Table("KULLANICI", Schema = "dbo")]
    public class Kullanici : BaseEntity
    {
        [Key]
        public Guid KULLANICI_ID { get; set; } //PK

        [StringLength(25)]
        public string KULLANICI_AD { get; set; } 

        [StringLength(200)]
        public string SIFRE { get; set; }

    }
}