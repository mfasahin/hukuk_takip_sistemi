using Presentation.Filters;
using System.Web.Mvc;

namespace Presentation
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RequireLoginAttribute());   
        }
    }
}
