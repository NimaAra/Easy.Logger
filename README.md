[![Build status](https://ci.appveyor.com/api/projects/status/k6ng7qdsd30c3nep?svg=true)](https://ci.appveyor.com/project/NimaAra/easy-logger)

# Easy Logger
A modern, high performance, cross platform wrapper around Log4Net.

Supports _.Net Core_ (_.Net 4_ & _netstandard1.3_) running on:
* .Net Core
* .Net Framework 4 and above
* Mono & Xamarin

Details and benchmarks [HERE](http://www.nimaara.com/2016/01/01/high-performance-logging-log4net/).
___


### Usage example:

Start by getting the singleton instance which by convention expects a valid `log4net.config` file at the root of your application:
```csharp
ILogService logService = Log4NetService.Instance;
```
If you need to configure **log4net** using an alternative configuration file, you can do so by:
```csharp
logService.Configure(new FileInfo("path-to-your-log4net-config-file"));
```

\* Any change to the log4net configuration file will be reflected immediately without the need to restart the application.

Now that you have an instance of the `ILogService`, you can get a logger using the `GetLogger()` method in three different ways:

##### Generics:
```csharp
IEasyLogger logger = logService.GetLogger<Program>();
logger.Debug("I am in Program!");
```
##### Type:
```csharp
logger = logService.GetLogger(typeof(MyService));
logger.Debug("I am in MyService!");
```
##### Plain string:
```csharp
logger = logService.GetLogger("Paradise");
logger.Debug("I am in Paradise!");
```

The above logging results in the the following log entries (based on the [sample](https://github.com/NimaAra/Easy.Logger/blob/master/Easy.Logger/sample-log4net.config) config file):

```
2016-06-29 00:11:24,590 [DEBUG] [ 1] Program - I am in Main!
2016-06-29 00:11:24,595 [DEBUG] [ 1] MyService - I am in MyService!
2016-06-29 00:11:24,595 [DEBUG] [ 1] Paradise - I am in Paradise!
```

### Scoped logging:
Sometimes you need to log the entry and exit points of an operation so instead of having to log that manually we can use the scoping feature of the library:

```csharp
using (logger.GetScopedLogger("SomeScope", EasyLogLevel.Debug))
{
    logger.Info("Foo is awesome!");
    logger.Debug("Bar is even more awesome!");
}
```
which produces the following log entries:
```
2016-06-29 00:15:51,577 [DEBUG] [ 1] Paradise - [-BEGIN---> SomeScope]
2016-06-29 00:15:51,578 [INFO ] [ 1] Paradise - Foo is awesome!
2016-06-29 00:15:51,578 [DEBUG] [ 1] Paradise - Bar is even more awesome!
2016-06-29 00:15:51,578 [DEBUG] [ 1] Paradise - [--END----> SomeScope]
```
\* The `ScopedLogger` is a `struct` and has been designed with performance in mind therefore frequent logging using this method does not put pressure on the GC or the CPU.

### Dependency Injection:

The library has been designed with DI in mind so as an example, given the service class and its corresponding interface below:

```csharp
public class MyService : IService
{
    private readonly IEasyLogger _logger;

    public MyService(ILogService logService)
    {
        _logger = logService.GetLogger(this.GetType());
    }

    public void Start()
    {
        _logger.Debug("I am started");
    }
}

public interface IService
{
    void Start();
}
```

Using your favourite IoC container you can then do:

```csharp
var container = new Container();
container.RegisterSingleton<ILogService>(logService);
container.Register<IService, MyService>();

var service = container.GetInstance<IService>();
service.Start();
```
Running the above results in the following log entry:

```
2016-06-29 00:15:51,661 [DEBUG] [ 1] MyService - I am started
```

### Disposal
The library does not need to be disposed explicitly as the framework takes care of the flushing of any pending log entries. In any case you can explicitly dispose the `Log4NetService` by:

```csharp
logService.Dispose();
```

* NuGet at: https://www.nuget.org/packages/Easy.Logger/
