using System;

namespace Presentation.Models
{
    public class IcraMahkemeModel
    {
        public Guid MahkemeId { get; set; } // PK

        public string MahkemeAd { get; set; } // NOT NULL
    }
}