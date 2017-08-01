namespace Microsoft.AspNetCore.Routing
{
    public class AbsolutPathData : VirtualPathData
    {
        public string Host { get; set; }
        public AbsolutPathData(IRouter router, string virtualPath, string host)
            : base(router, virtualPath)
        {
            Host = host;
        }
        public AbsolutPathData(IRouter router, string virtualPath, RouteValueDictionary dataTokens, string host)
            : base(router, virtualPath, dataTokens)
        {
            Host = host;
        }
    }
}
