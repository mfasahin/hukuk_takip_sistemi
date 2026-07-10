using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class KullaniciModel
    {
        public int KULLANICI_ID { get; set; } //PK

        public string KULLANICI_AD { get; set; } // NOT NULL

        public string SIFRE { get; set; } // NOT NULL
    }
}