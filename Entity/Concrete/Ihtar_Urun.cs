using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
    public class IhtarUrun
    {
        [Key]
        public int IHTAR_URUN_ID { get; set; } // PK

        [Required]
        public int URUN_ID { get; set; } // FK

        [Required]
        public int IHTAR_ID { get; set; } // FK

        // Navigation Properties
        public Urun Urun { get; set; }
        public Ihtar Ihtar { get; set; }
    }
}
