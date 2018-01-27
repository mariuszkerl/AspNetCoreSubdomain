<img src="https://loled.blob.core.windows.net/images/subdomain-thumb.png" alt="logo" />

[![NuGet](https://img.shields.io/nuget/v/AspNetCoreSubdomain.svg)](https://www.nuget.org/packages/AspNetCoreSubdomain/)

# ASP.NET Subdomain Routing
Goal of that lib is to make subdomain routing easy to use in asp net core mvc applications. Normally you would use some custom route for some special case scenario in your app. This should solve most of issues while using subdomain routing. Inspired by couple of already existing libraries around the web which handle routing in some degree this should meet requirements:

1. Register subdomain routes as you would do with normal routes.
2. Make links, forms urls etc. in views as you would do with helpers in your cshtml pages.
3. Catch all route values in controller.

### Continuous Integration
| Build server                | Build status                                                                                                                                                        | Unit tests                                                                                                                                                   | Integration tests                                                                                                                                                   | Functional tests                                                                                                                                                   |
|-----------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| AppVeyor                    | [![Build status](https://ci.appveyor.com/api/projects/status/qmob3plw4quw90ny/branch/master?svg=true)](https://ci.appveyor.com/project/mariuszkerl/aspnetcoresubdomain-u73ra/branch/master)                  | [![Build status](https://ci.appveyor.com/api/projects/status/3obt7r9yi4jgqblp/branch/master?svg=true)](https://ci.appveyor.com/project/mariuszkerl/aspnetcoresubdomain-s4142/branch/master) | [![Build status](https://ci.appveyor.com/api/projects/status/cuqlv91ogsyil6bi/branch/master?svg=true)](https://ci.appveyor.com/project/mariuszkerl/aspnetcoresubdomain/branch/master)  | [![Build status](https://ci.appveyor.com/api/projects/status/j8v2jc6muxai92jb/branch/master?svg=true)](https://ci.appveyor.com/project/mariuszkerl/aspnetcoresubdomain-07mgu/branch/master)

## Wiki
https://github.com/mariuszkerl/AspNetCoreSubdomain/wiki

## Setup
### Startup.cs

Your application have to be aware of using subdomains. Important thing is to use method ```AddSubdomains()``` before ```AddMvc()```
```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Add framework services.
    //...
    services.AddSubdomains();
    services.AddMvc();
}
```
You configure your routes just like standard routes, but you cannot use standard ```MapRoute``` methods. That will be explained later in wiki. Use MapRoute method from this lib extensions method which accepts ```hostnames``` as a parameter.
```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
    var hostnames = new[] { "localhost:54575" };
    
    app.UseMvc(routes =>
    {
        routes.MapRoute(
            hostnames,
            "NormalRoute",
            "{action}",
            new { controller = "SomeController" });

        routes.MapSubdomainRoute(
            hostnames,
            "SubdomainRoute",
            "{controller}", //that's subdomain parameter, it can be anything
            "{action}",
            new { controller = "Home", action = "Action1" });
    )};
}
```
## Usage
### Your .cshtml files
Goal of that library is not only catching routes for subdomain but also generating links to actions while persisting standard razor syntax. Helper below will generate url ```<a href="http://home.localhost:54575">Hyperlink example</a>```. Route named SubdomainRoute should catch that link.
```csharp
@Html.ActionLink("Hyperlink example", "Action1", "Home")
```

### Controller
Big  advantage of library is you can catch all route values with controller.
```csharp
//HomeController.cs
public IActionResult Action1()
{
    //code
}
```

Having url ```http://home.localhost:54575/``` will invoke ```Action1``` method in ```Home``` controller.

## Running samples project
https://github.com/mariuszkerl/AspNetCoreSubdomain/wiki/Running-samples-project
