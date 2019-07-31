### Setting up in ASP.NET Core:

The current implementation is based on [log4net](https://logging.apache.org/log4net/).

```csharp
internal class Program
{
    private static void Main()
    {
        using (var host = new WebHostBuilder()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseKestrel()
            .UseUrls("http://*:8081")                
            .ConfigureLogging(Startup.ConfigureLogging)
            .Configure(Startup.Configure)
            .Build())
        {
            host.Run();
        }
    }
}

internal static class Startup
{
    public static void ConfigureLogging(WebHostBuilderContext ctx, ILoggingBuilder builder)
    {
        builder.AddConfiguration(ctx.Configuration.GetSection("Logging"));
        if (ctx.HostingEnvironment.IsDevelopment()) 
        { 
            builder.AddConsole(); 
        }

        // Use the default log
        builder.AddEasyLogger();

        /*
         var configFile = new FileInfo("sample-log4net.config");
         builder.AddEasyLogger(configFile);
        */
    }

    public static void Configure(IApplicationBuilder app) { ... }
}
```