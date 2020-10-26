using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookish.Web.Helpers
{
    public static class ViewContextExtension
    {
        public static string ViewActiveStatus(this ViewContext context, string viewName)
        {
            return context.RouteData.Values["Action"].ToString() == viewName ? "active" : "";
        }
    }
}
