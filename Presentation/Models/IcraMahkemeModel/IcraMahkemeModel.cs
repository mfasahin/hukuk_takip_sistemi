using System;

namespace Presentation.Models
{
    public class IcraMahkemeModel : BaseModel   
    {
        public Guid MahkemeId { get; set; } // PK

        public string MahkemeAd { get; set; } // NOT NULL
    }
}