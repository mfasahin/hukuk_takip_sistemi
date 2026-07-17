    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dto
{
    public class IhtarDto
    {
        public int IhtarId { get; set; }
        public decimal BorcTutar { get; set; }
        //public DateTime IhtarTarih { get; set; }
        public string MusteriAd { get; set; }
        public string AvukatAd { get; set; }
        public string SubeAd { get; set; }
    }

}
