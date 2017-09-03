namespace Microsoft.AspNetCore.Routing
{
    public class AbsolutPathData : VirtualPathData
    {
        public string Host { get; set; }

        public string Protocol { get; set; }

        public AbsolutPathData(IRouter router, string virtualPath, string host, string protocol)
            : base(router, virtualPath)
        {
            Host = host;
            Protocol = protocol;
        }
        public AbsolutPathData(IRouter router, string virtualPath, RouteValueDictionary dataTokens, string host, string protocol)
            : base(router, virtualPath, dataTokens)
        {
            Host = host;
            Protocol = protocol;
        }
    }
}
