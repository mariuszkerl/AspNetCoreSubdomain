using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace TestHelpers
{
    public static class ActionContextVisitor
    {
        public static void Visit(ActionContext actionContext, IRouteBuilder routeBuilder, string action, string controller, string area)
        {
            actionContext.RouteData = new RouteData();
            actionContext.RouteData.Values.Add(nameof(action), action);
            actionContext.RouteData.Values.Add(nameof(controller), controller);
            if (area != null)
            {
                actionContext.RouteData.Values.Add(nameof(area), area);
            }
            actionContext.RouteData.Routers.Add(routeBuilder.Build());
        }
    }
}