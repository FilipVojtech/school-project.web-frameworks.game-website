using Microsoft.AspNetCore.Mvc.Rendering;

namespace Website.Helpers;

public static class HtmlHelpers
{
    public static string IsSelected(this IHtmlHelper html, string controllers = "", string actions = "", string cssClass = "selected")
    {
        string currentAction = html.ViewContext.RouteData.Values["action"] as string;
        string currentController = html.ViewContext.RouteData.Values["controller"] as string;

        IEnumerable<string> acceptedActions = (actions ?? currentAction).Split(',');
        IEnumerable<string> acceptedControllers = (controllers ?? currentController).Split(',');

        return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ?
            cssClass : string.Empty;
    }

}