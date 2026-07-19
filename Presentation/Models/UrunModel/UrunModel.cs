using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class UrunModel
    {
        public Guid UrunId { get; set; } //PK

        public string UrunAd { get; set; } // NOT NULL

        public string UrunKod { get; set; } // NOT NULL

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? SonGecerlilikTar { get; set; }

        public DateTime? GrsTarZmn { get; set; }

        public DateTime? GncTarZmn { get; set; }

        public DateTime? SilTarZmn { get; set; }
    }
}