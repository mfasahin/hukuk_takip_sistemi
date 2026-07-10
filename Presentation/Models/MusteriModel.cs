using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class MusteriModel
    {
        public int MUSTERI_ID { get; set; }   // PK

        public string MUST_NO { get; set; }   // NOT NULL

        public string MUST_AD { get; set; }   // NOT NULL

        public string MUST_SOYAD { get; set; }

        public string MUST_KIMLIK_NO { get; set; }


        public string MUST_VKNO { get; set; } //düzeltildi

        public string MUST_EPOSTA { get; set; }

        public string MUST_TEL_NO { get; set; }

    }
}