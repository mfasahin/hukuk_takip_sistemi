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
        public UrunModel Urun { get; set; }

        public int IhtarId { get; set; } // FK

        public IhtarModel Ihtar { get; set; }
    }
}