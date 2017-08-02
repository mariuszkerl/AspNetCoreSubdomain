# ASP.NET Subdomain Routing
Goal of that lib is to make subdomain routing easy to use in asp net core mvc applications. Normally you would use some custom route for some special case scenario in your app. This should solve most of issues while using subdomain routing. Inspired by couple of already existing libraries around the web which handle routing in some degree this should meet requirements:

1. Register subdomain routes as you would do with normal routes.
2. Make links in views as you would do with helpers in your cshtml pages.
3. Catch all route values in controller.

## Desired usage
It is important to note that order of routes is the same as standard asp net mvc routing with couple exceptions.

1. Subdomain routes have to be declared before normal routes.
2. Static subdomain routes have to be declared as last subdomain routes.
3. You have to declare used hostnames in your application since todays subdomain diversity is really wide and it would be impossible to predefine those (it would be also insufficient).
### Startup.cs

You configure your routes just like standard routes, but with respect to rules above.
```csharp

var hostnames = new[] { "localhost" };

//example route http://subdomain.localhost/1
routes.MapSubdomainRoute(
    hostnames,
    "SubdomainExamle1",
    "{parameterInSubdomain}",
    "{id}",
    new { controller = "Home", action = "Action1" });

    
//example route http://subdomain.localhost/
//example route http://subdomain.localhost/somecontroller
//example route http://subdomain.localhost/somecontroller/someaction
routes.MapSubdomainRoute(
    hostnames,
    "SubdomainExample2",
    "{parameterInSubdomain}",
    "{controller}/{action}",
    new { controller = "Home", action = "Action2" });

//static subdomain
//example route http://subdomain1.localhost/home/action3/1
routes.MapSubdomainRoute(
    hostnames,
    "SubdomainExample3",
    "subdomain1",
    "{controller}/{action}/{id}");

//static subdomain
//example route http://subdomain2.localhost/
routes.MapSubdomainRoute(
    hostnames,
    "SubdomainExample4",
    "subdomain2",
    "{controller}/{action}",
    new { controller = "Home", action = "Action4" });
```

### Your .cshtml files
Goal of that library is not only catching routes for subdomain but also generating links to actions while persisting standard razor syntax. Helper below will generate url ```<a href="http://subdomain1.localhost/1">Hyperlink example</a>```. Route named SubdomainExample1 should catch that link and contain route values in ```RouteData.Values```
```csharp
@Html.SubdomainLink("Hyperlink example", "Action1", new { parameterInSubdomain = "subdomain1", id ="1" })
```

### Controller
Big  advantage of library is you can catch all route values with controller.
```csharp
public IActionResult Action1(parameterInSubdomain, id)
{
    //code
}
```

Having url ```http://subdomain1.localhost/1``` will invoke ```Action1``` method in ```Home```.