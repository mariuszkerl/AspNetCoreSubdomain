namespace Microsoft.AspNetCore.Routing
{
    /// <summary>
    /// Represents information about the route, virtual path, host and protocol that are the result of
    /// generating a URL with the ASP.NET routing middleware. Extends from <see cref="VirtualPathData"/>.
    /// </summary>
    public class AbsolutPathData : VirtualPathData
    {
        /// <summary>
        /// Gets or sets the host that was generated from the <see cref="VirtualPathData.Router"/>.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the protocol that was generated from the <see cref="VirtualPathData.Router"/>.
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbsolutPathData"/>.
        /// </summary>
        /// <param name="router">The object that is used to generate the URL.</param>
        /// <param name="virtualPath">The generated URL.</param>
        /// <param name="host">The generated host.</param>
        /// <param name="protocol">The generated protocol.</param>
        public AbsolutPathData(IRouter router, string virtualPath, string host, string protocol)
            : base(router, virtualPath)
        {
            Host = host;
            Protocol = protocol;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbsolutPathData"/>
        /// </summary>
        /// <param name="router">The object that is used to generate the URL.</param>
        /// <param name="virtualPath">The generated URL.</param>
        /// <param name="dataTokens">The collection of custom values.</param>
        /// <param name="host">The generated host.</param>
        /// <param name="protocol">The generated protocol.</param>
        public AbsolutPathData(IRouter router, string virtualPath, RouteValueDictionary dataTokens, string host, string protocol)
            : base(router, virtualPath, dataTokens)
        {
            Host = host;
            Protocol = protocol;
        }
    }
}
