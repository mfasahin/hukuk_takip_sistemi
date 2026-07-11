using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class SubeModel
    {
        public int SUBE_ID { get; set; } //PK

        public string SUBE_KODU { get; set; } // NOT NULL

        public string SUBE_ADI { get; set; } // NOT NULL

        public string SUBE_BOLGE { get; set; }

        public string SUBE_ADRES { get; set; }

        public string SUBE_TEL_NO { get; set; }
    }
}