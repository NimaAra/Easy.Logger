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

            var logFile = new FileInfo("sample-log4net.config");
            builder.AddProvider(new EasyLoggerProvider(logFile));
        }

        public static void Configure(IApplicationBuilder app) { ... }
    }
```

Alternatively, you can skip the `ConfigureLogging` and use the extension methods on `ILoggerFactory` instead:
```csharp
internal static class Startup
{
    public static void Configure(IApplicationBuilder app)
    {
        var loggerFactory =  app.ApplicationServices.GetService<ILoggerFactory>();
        
        // Looks for a valid log4net.config file in the executing directory.
        // You can also pass in a FileInfo to your config else where.
        loggerFactory.AddEasyLogger();
            
        var logger = loggerFactory.CreateLogger("Configure.Startup");
        logger.LogDebug("What a beautiful day!");
            
        app.Run(c => c.Response.WriteAsync("<html><body><span>24</span></body></html>"));
    }
}
```