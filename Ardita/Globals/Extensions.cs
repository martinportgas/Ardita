using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ardita.Globals
{
    public static class Extensions
    {
        public static string IsSelected(this IHtmlHelper html, string area = null, string controller = null, string action = null)
        {
            string cssClass = "active";
            string currentArea = (string)html.ViewContext.RouteData.Values["area"];
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(area))
                area = currentArea;

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return area == currentArea && controller == currentController ?
                cssClass : String.Empty;
        }
    }
}
