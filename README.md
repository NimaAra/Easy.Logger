[![Build status](https://ci.appveyor.com/api/projects/status/k6ng7qdsd30c3nep?svg=true)](https://ci.appveyor.com/project/NimaAra/easy-logger)<br>

### NuGet
[![NuGet](https://img.shields.io/nuget/v/Easy.Logger.svg?label=Easy.Logger)](https://www.nuget.org/packages/Easy.Logger)
[![NuGet](https://img.shields.io/nuget/v/Easy.Logger.Interfaces.svg?label=Easy.Logger.Interfaces)](https://www.nuget.org/packages/Easy.Logger.Interfaces)
[![NuGet](https://img.shields.io/nuget/v/Easy.Logger.Extensions.svg?label=Easy.Logger.Extensions)](https://www.nuget.org/packages/Easy.Logger.Extensions)
[![NuGet](https://img.shields.io/nuget/v/Easy.Logger.Extensions.Microsoft.svg?label=Easy.Logger.Extensions.Microsoft)](https://www.nuget.org/packages/Easy.Logger.Extensions.Microsoft)

# Easy Logger
A modern, high performance, cross platform wrapper for Log4Net.

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
[2016-06-29 00:11:24,590] [DEBUG] [ 1] [Program] - I am in Main!
[2016-06-29 00:11:24,595] [DEBUG] [ 1] [MyService] - I am in MyService!
[2016-06-29 00:11:24,595] [DEBUG] [ 1] [Paradise] - I am in Paradise!
```

### Scoped logging:
Sometimes you need to add context to your log entries for example when adding a _Correlation ID_ to identify a request. Instead of having to do this manually, we can leverage the scoping feature of the library:

```csharp
const string REQUEST_ID = "M1351";
using (logger.GetScopedLogger($"[{REQUEST_ID}]"))
{
    logger.Info("Foo is awesome.");
    logger.Debug("Bar is even more awesome!");
}
```
which produces the following log entries:
```
[2017-09-17 17:39:16,573] [INFO ] [ 1] [Program] - [M1351] Foo is awesome.
[2017-09-17 17:39:16,575] [DEBUG] [ 1] [Program] - [M1351] Bar is even more awesome!
```
The scoping feature has been implemented with performance in mind and is encouraged to be used instead of the [_log4net_'s _Nested Diagnostic Context (NDC)_](https://logging.apache.org/log4net/log4net-1.2.11/release/sdk/log4net.NDC.html). _NDC_ requires `Fixing` the _Properties_ which results in a significantly lower performance.

### Dependency Injection:

The library has been designed with _DI_ in mind so as an example, given the service class and its corresponding interface below:

```csharp
public class MyService : IService
{
    private readonly IEasyLogger _logger;

    public MyService(ILogService logService) 
        => _logger = logService.GetLogger(this.GetType());

    public void Start(() => _logger.Debug("I am started"));
}

public interface IService
{
    void Start();
}
```

Using your favorite _IoC_ container you can do:

```csharp
var container = new Container();
container.RegisterSingleton<ILogService>(logService);
container.Register<IService, MyService>();

var service = container.GetInstance<IService>();
service.Start();
```

The above can be even further simplified by injecting the [`IEasyLogger<T>`](https://github.com/NimaAra/Easy.Logger/blob/bf8e0e4caa1443562438c18a2d29f4bc09407ec0/Easy.Logger.Interfaces/IEasyLogger.cs#L403) directly instead of the `ILogService`:

```csharp
public class MyService : IService
{
    private readonly IEasyLogger _logger;

    public MyService(IEasyLogger<MyService> logger) => _logger = logger;

    public void Start(() => _logger.Debug("I am started"));
}
```

If your _IoC_ container supports [mapping generic interfaces to generic implementations](https://simpleinjector.readthedocs.io/en/latest/advanced.html#registration-of-open-generic-types), e.g. [Simple Injector](https://simpleinjector.org), then you can further simplify the registration by:

```csharp
container.Register(typeof(IEasyLogger<>), typeof(Log4NetLogger<>));
```

Running any of the flavors above results in the following log entry:

```
[2016-06-29 00:15:51,661] [DEBUG] [ 1] [MyService] - I am started
```

### Disposal
The library does not need to be disposed explicitly as the framework takes care of the flushing of any pending log entries. In any case you can explicitly dispose the `Log4NetService` by:

```csharp
logService.Dispose();
```

## ASP.NET Core Integration
_Easy.Logger_ can be used in *ASP.NET Core* by referencing the [_Easy.Logger.Extensions.Microsoft_]((https://www.nuget.org/packages/Easy.Logger.Extensions.Microsoft)) _NuGet_ package. For more details on how to get started, take a look at [HERE](https://github.com/NimaAra/Easy.Logger/blob/master/Easy.Logger.Extensions.Microsoft/README.md).

## Easy Logger Extensions
The [_Easy.Logger.Extensions_*](https://github.com/NimaAra/Easy.Logger/tree/master/Easy.Logger.Extensions) package offers more functionality to extend _log4net_. The package currently contains the [HTTPAppender](https://github.com/NimaAra/Easy.Logger/blob/master/Easy.Logger.Extensions/HTTPAppender.cs) which uses the `HTTPClient` to _POST_ _JSON_ payloads of log events to an endpoint. Take the following configuration as an example:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<log4net>
  <root>
    <level value="ALL"/>
    <appender-ref ref="AsyncBufferingForwarder"/>
  </root>

  <appender name="AsyncBufferingForwarder" type="Easy.Logger.AsyncBufferingForwardingAppender, Easy.Logger">
    <lossy value="false" />
    <bufferSize value="512" />
    
    <idleTime value="500" />
    <fix value="Message, ThreadName, Exception" />
  
    <appender-ref ref="RollingFile"/>
    <appender-ref ref="HTTPAppender"/>
  </appender>

  <appender name="RollingFileAppender" type="log4net.Appender.RollingFileAppender">
    <file type="log4net.Util.PatternString" value="App-%date{yyyy-MM-dd}.log" />
    <appendToFile value="false"/>
    <rollingStyle value="Composite"/>
    <maxSizeRollBackups value="-1"/>
    <maximumFileSize value="50MB"/>
    <staticLogFileName value="true"/>
    <datePattern value="yyyy-MM-dd"/>
    <preserveLogFileNameExtension value="true"/>
    <countDirection value="1"/>
    <layout type="log4net.Layout.PatternLayout">
      <conversionPattern value="[%date{ISO8601}] [%-5level] [%2thread] [%logger{1}] - %message%newline%exception"/>
    </layout>
  </appender>

  <appender name="HTTPAppender" type="Easy.Logger.Extensions.HTTPAppender, Easy.Logger.Extensions">
    <name value="SampleApp" />
    <endpoint value="http://localhost:1234" />
    <includeHost value="true" />
  </appender>
```

The configuration specifies two appenders:
1. RollingFileAppender
2. HTTPAppender

Both of these appenders have been placed inside the `AsyncBufferingForwardingAppender` which buffers and batches up the events then pushes the entries to each of them in order.

So in addition to the log events being persisted to a log file they are also serialized to _JSON_ and asynchronously _POSTed_ to an endpoint specified at: `http://localhost:1234`. All of this has been implemented in an efficient manner to reduce _GC_ and overhead.

A sample Web server (not suitable for production) which only accepts valid log payloads has been implemented as [`EasyLogListener`](https://github.com/NimaAra/Easy.Logger/blob/master/Easy.Logger.Tests.Integration/EasyLogListener.cs) and the models for deserializing the _JSON_ payload can be found in [`LogPayload`](https://github.com/NimaAra/Easy.Logger/blob/master/Easy.Logger.Tests.Integration/Models/LogPayload.cs). For more info on sending and receiving the log messages, take a look at the [Easy.Logger.Tests.Integration](https://github.com/NimaAra/Easy.Logger/tree/master/Easy.Logger.Tests.Integration) project.

Given the following log entries:

```csharp
logger.Debug("Something is about to happen...");
logger.InfoFormat("What's your number? It's: {0}", 1234);
logger.Error("Ooops I did it again!", new ArgumentNullException("cooCoo"));
logger.FatalFormat("Going home now {0}", new ApplicationException("CiaoCiao"));
```

The following payload will then be received at the endpoint:

```json
{
  "pid": "12540",
  "processName": "SampleApp.x32",
  "host": "TestServer-01",
  "sender": "SampleApp",
  "timestampUTC": "2017-09-17T16:27:14.9377168+00:00",
  "batchNo": 1,
  "entries": [
    {
      "dateTimeOffset": "2017-09-17T17:27:14.4107128+01:00",
      "loggerName": "Sample.Program",
      "level": "DEBUG",
      "threadID": "1",
      "message": "Something is about to happen...",
      "exception": null
    },
    {
      "dateTimeOffset": "2017-09-17T17:27:14.4147491+01:00",
      "loggerName": "Sample.Program",
      "level": "INFO",
      "threadID": "1",
      "message": "What's your number? It's: 1234",
      "exception": null
    },
    {
      "dateTimeOffset": "2017-09-17T17:27:14.4182507+01:00",
      "loggerName": "Sample.Program",
      "level": "ERROR",
      "threadID": "1",
      "message": "Ooops I did it again!",
      "exception": {
        "ClassName": "System.ArgumentNullException",
        "Message": "Value cannot be null.",
        "Data": null,
        "InnerException": null,
        "HelpURL": null,
        "StackTraceString": null,
        "RemoteStackTraceString": null,
        "RemoteStackIndex": 0,
        "ExceptionMethod": null,
        "HResult": -2147467261,
        "Source": null,
        "WatsonBuckets": null,
        "ParamName": "cooCoo"
      }
    },
    {
      "dateTimeOffset": "2017-09-17T17:27:14.4187508+01:00",
      "loggerName": "Sample.Program",
      "level": "FATAL",
      "threadID": "1",
      "message": "Going home now System.ApplicationException: CiaoCiao",
      "exception": null
    }
  ]
}
```

<p>* Requires minimum <i>NET 4.5</i> or <i>netstandard1.3</i>.</p>