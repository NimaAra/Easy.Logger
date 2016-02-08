namespace Easy.Logger
{
    using System;
    using System.IO;

    public interface ILogService : IDisposable
    {
        void Configure(FileInfo configFile);
        ILogger GetLogger(string logName);
        ILogger GetLogger(Type logClass);
        ILogger GetLogger<T>();
    }
}