using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class UrunModel
    {
        public int URUN_ID { get; set; } //PK

        public string URUN_AD { get; set; } // NOT NULL

        public string URUN_KOD { get; set; } // NOT NULL

        public DateTime? SON_GECERLILIK_TAR { get; set; }
        
        public DateTime? GRS_TAR_ZMN{  get; set; }

        public DateTime? GNC_TAR_ZMN{  get; set; }

        public DateTime? SIL_TAR_ZMN {  get; set; }
    }
}