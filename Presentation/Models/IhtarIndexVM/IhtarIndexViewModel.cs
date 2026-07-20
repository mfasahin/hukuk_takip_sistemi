using System.Collections.Generic;
using System.Web.Mvc;

namespace Presentation.Models
{
    public class IhtarIndexViewModel
    {
        public List<IhtarModel> Ihtarlar { get; set; }

        public List<SelectListItem> MusteriList { get; set; }
        public List<SelectListItem> SubeList { get; set; }
        public List<SelectListItem> AvukatList { get; set; }
        public List<SelectListItem> UrunList { get; set; }
    }
}}