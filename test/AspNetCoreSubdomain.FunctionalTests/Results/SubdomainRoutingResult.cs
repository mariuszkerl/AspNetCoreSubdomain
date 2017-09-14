using System.Collections.Generic;

internal class SubdomainRoutingResult
{
    public string[] ExpectedUrls { get; set; }

    public Dictionary<string, object> RouteValues { get; set; }

    public string RouteName { get; set; }

    public string Action { get; set; }

    public string Controller { get; set; }
}