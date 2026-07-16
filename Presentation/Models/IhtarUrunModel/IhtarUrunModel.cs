using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class IhtarUrunModel
    {
        public int IhtarUrunId { get; set; } // PK

        public int UrunId { get; set; } // FK

        public int IhtarId { get; set; } // FK
    }
}