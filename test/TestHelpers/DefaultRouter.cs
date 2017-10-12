using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace TestHelpers
{
    public class DefaultRouter : IRouter
    {
        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            return null;
        }

        public Task RouteAsync(RouteContext context)
        {
            context.Handler = (c) => Task.FromResult(0);
            return Task.FromResult(false);
        }
    }
}
