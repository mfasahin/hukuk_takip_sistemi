using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class MusteriModel
    {
        public int MusteriId { get; set; }   // PK

        public string MustNo { get; set; }   // NOT NULL

        public string MustAd { get; set; }   // NOT NULL

        public string MustSoyad { get; set; }

        public string MustKimlikNo { get; set; }

        public string MustVknNo { get; set; } 

        public string MustEposta { get; set; }

        public string MustTelNo{ get; set; }

        public DateTime? GrsTarZmn { get; set; }

        public DateTime? GncTarZmn { get; set; }

        public DateTime? SilTarZmn { get; set; } // soft delete için

    }
}