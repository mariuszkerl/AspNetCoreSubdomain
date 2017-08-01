# ASP.NET Subdomain Routing

## Desired usage
```csharp
var hostnames = new[] { "localhost" };
routes.MapSubdomainRoute(
    hostnames,
    "SubdomainExamle",
    "{parameterInSubdomain}",
    "{id}",
    new { controller = "Home", action = "Parameters" });
```
