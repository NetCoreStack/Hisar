## Hisar
Cross-Platform Modular Component Development Infrastructure 

[![NuGet](https://img.shields.io/nuget/v/NetCoreStack.Hisar.svg?longCache=true&style=flat-square)](https://www.nuget.org/packages/NetCoreStack.Hisar)
[![NuGet](https://img.shields.io/nuget/dt/NetCoreStack.Hisar.svg?longCache=true&style=flat-square)](https://www.nuget.org/packages/NetCoreStack.Hisar)


>[Latest release on Nuget](https://www.nuget.org/packages/NetCoreStack.Hisar/)


## Why Hisar?
The purpose of this framework is that eliminate the **monolithic application challenges** and allow us to develop component (module) without main application dependency. Thus the component can be run and test as a standalone web application. When you complete development requirements and ready to deploy the component then just pack it or copy the output to the related directory of the main application.

> **MsBuild** , **NuGet** or **[dotnet pack](https://docs.microsoft.com/en-us/dotnet/articles/core/tools/dotnet-pack)** tools can be used for packaging. In this way, the component will be able to part of the main hosting application and available to upload **NuGet** or **MyGet** package repositories. 

Despite the other framework you don't need to reference any component (module) to main application but if you want the component to act as part of the main application you can copy the output .dll to **ExternalComponents** folder of main application so the **Hisar** platform engine resolves all dependencies, controllers, views, contents and then registers the component with convention name.

#### Controllers

For example; you got a component which is name **Hisar.Component.Carousel**. For convention, Hisar engine resolves that namespace as a **Carousel** component when to act as a part of the main application. For this component, the component **Controllers** will work as an **Area** with http://\<domainname\>/carousel URL prefix in main application (Landing Hosting or Admin Hosting apps).

#### ViewComponents
[View components]((https://docs.microsoft.com/en-us/aspnet/core/mvc/views/view-components)) are similar to partial views, but they are much more powerful. Hisar ViewComponents work as follows;

```csharp
@section styles {
    <link rel="stylesheet" href="@Url.ComponentContent("~/css/carousel.css")" />
}

<h2>Carousel</h2>
@await Component.InvokeAsync(ViewContext.ResolveName("Carousel"))
```
    ViewContext.ResolveName("Carousel")

extension method creates convention name for the executing view component according to running component state. In this way, the Component (module, which has CarouselViewComponent) works on its own and resolve the convenient name for any state. (Standalone app or part of the main application)

#### Component Contents
```csharp
<img src="@Url.ComponentContent("~/img/banner1.svg")" alt="ASP.NET Core" class="img-responsive" />
```

    Url.ComponentContent("~/img/banner1.svg")

this extension method makes the content accessible for the specified path for any state.

#### Startup.cs

**Startup.cs** is the bootstrap - starter class for the ASP.NET Core web application. We tried to preserve as much as possible in the same way this file except 

    public static void ConfigureRoutes(IRouteBuilder routes)

for the method mentioned above and

    .UseStartup<DefaultHisarStartup<Startup>>()

our custom startup wrapper to handle component and application acts. For more details, you can check the test projects source code in this repository.

## Component Development
To develop the component you can fallow these steps;
 - Download the .NET Core SDK 2.2.0 or newer. Once installed, run this command:
 
        dotnet tool install --global dotnet-hisar --version 2.2.0

 - Create a web application with Hisar.Component prefix. (Hisar.Component.YourComponentName)
 - Add NetCoreStack.Hisar package to the project.

        dotnet add package NetCoreStack.Hisar

 - Update the line UseStartup\<Startup> to UseStartup\<DefaultHisarStartup\<Startup>> for WebHostBuilder.

 - Add PreBuild event to generate component helper classes with Hisar global tool.
    ```xml
    <Target Name="PreBuild" BeforeTargets="PreBuildEvent">
        <Exec Command="cd $(ProjectDir) &amp; dotnet hisar --build &quot;$(ProjectDir)&quot;" />
    </Target>
    ```

- Add Hisar Web Cli Services (Tooling)
    ```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        #if !RELEASE
        /// <param name="webCliAddress">Default is localhost:1444 (WebCli Tools)</param> 
        /// <param name="enableLiveReload">To enable livereload proxy</param>
        services.AddCliSocket<Startup>();
        #endif
        
        // ...
    }

    public void Configure(IApplicationBuilder app, ...)
    {
        #if !RELEASE
        app.UseCliProxy();
        #endif
        
        // ...
    }

    ```

 - Add all the required contents as embedded resources.
    ```xml
    <ItemGroup>
        <EmbeddedResource Include="Views\**\*.cshtml" />
        <EmbeddedResource Include="wwwroot\**\*.*" />
        <None Update="wwwroot\**\*">
            <CopyToPublishDirectory>PreserveNewest</CopyToPublishDirectory>
        </None>
    </ItemGroup>
    ```
 - If you have any templating directory for static serve files or hosting main web application, start the Web Cli tool on project root directory from command line. It will start the Web Cli application on **http://localhost:1444** to manage all main application contents or provide **_Layout.cshtml** if no appdir option specified. (One working Web Cli tool is enough for same templating usage)

        dotnet hisar --appdir <the-path-of-your-hosting-app-relative-or-absolute> 

## Database
### MongoDb Database 

    docker volume create --name=mongodata

    docker run -it -v mongodata:/data/db -p 27017:27017 -d mongo

## Tools
[Hisar Web Cli](https://github.com/NetCoreStack/Tools) tool provides manage extensibility and templating of components. Dotnet global tools extensibility model has various tooling features. **Hisar Web Cli** is built on top of it.

## Components.json (sample nuget package reference)

    {
        "components": {
            "Hisar.Component.CoreManagement": {
                "targetFramework": "netcoreapp2.2",
                "version": "2.2.0"
            }
        }
    }

## TODO
 - Hisar Package Repository
 - Component Marketplace (Upload, search, download and enable)

## Contributing to Repository
 - Fork and clone locally.
 - Build the solution with Visual Studio 2017.
 - Create a topic specific branch in git.
 - Send a Pull Request.

## Prerequisites
> [ASP.NET Core](https://github.com/aspnet/Home)

## License
> The [MIT licence](https://github.com/NetCoreStack/Hisar/blob/master/LICENSE.txt)
