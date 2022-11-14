using System.Web;
using System.Web.Mvc;
using ThirdPartyEventEditor.Filters;

namespace ThirdPartyEventEditor
{
    /// <summary>
    /// Class for filters.
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Added global filters.
        /// </summary>
        /// <param name="filters">All global filters.</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ApplicationExceptionFilter());
        }
    }
}
