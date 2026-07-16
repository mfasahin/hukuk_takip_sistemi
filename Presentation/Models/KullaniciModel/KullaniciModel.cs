using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class KullaniciModel
    {
        public int KullaniciId { get; set; } //PK

        public string KullaniciAd { get; set; } // NOT NULL

        public string Sifre { get; set; } // NOT NULL
    }
}