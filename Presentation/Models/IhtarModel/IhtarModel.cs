using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class IhtarModel
    {
        public int IhtarId {  get; set; }
        public int MusteriId { get; set; }
        public string MusteriAd { get; set; }

        public int SubeId { get; set; }
        public string SubeAd { get; set; }

        public int AvukatId { get; set; }
        public string AvukatAd { get; set; }

        public decimal BorcTutar { get; set; } // DECIMAL(12,2)

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime IhtarTarZmn { get; set; } 

        public DateTime? GrsTarZmn { get; set; }

        public DateTime? GncTarZmn { get; set; }

        public DateTime? SilTarZmn { get; set; }

    }
}