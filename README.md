### Hisar: Cross-Platform Modular Component Development Infrastructure

There are tons of plugin, extension and component base application development framework.
They provide modular - plugin base extensibility, have great features and communities.
We are trying to do something little different to make .NET Core stack more fun!

This framework allows us to develop component (module) without main application dependency and can run, test and deploy it as a standalone web application or part of the main hosting site.

Dispite the other framework you don't need to reference any component to main application but if you want the component to act as part of the main application you can copy the output .dll to ExternalComponents folder of main application so the Hisar platform engine resolves all dependencies, controllers, views and then registers the component with convention name. 

For example; you got a component which is name Hisar.Component.Guideline. For convention Hisar engine resolves that namaspace as a **Guideline** component when act as a part of the main application. In this component controllers will work as an **Area** with http://mainsitedomain/guideline URL prefix.

**Startup.cs** is the bootstrap - starter class for the ASP.NET Core web application. We tried to preserve as much as possible in the same way that file except 

    public static void ConfigureRoutes(IRouteBuilder routes)

for the method mentioned above and

    .UseStartup<DefaultHisarStartup<Startup>>()

our custom startup wrapper to handle component and application acts. For more details you can check the test projects source code in this repository.

### Tools
[Hisar Web Cli](https://github.com/NetCoreStack/Tools) tools provide manage extensibility and templating of components. You don't need extra gulp or grunt tooling and scripting behaviors. .NET Core CLI tools extensibility model has various tooling features. **Hisar Web Cli** is built on top of it.


[Latest release on Nuget](https://www.nuget.org/packages/NetCoreStack.Hisar/)

### Prerequisites
> [ASP.NET Core](https://github.com/aspnet/Home)