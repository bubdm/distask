using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Distask
{
    internal sealed class NopLogger<T> : ILogger<T>
    {
        public IDisposable BeginScope<TState>(TState state) => new Disposable();

        public bool IsEnabled(LogLevel logLevel) => false;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
        }

        class Disposable : IDisposable
        {
            public void Dispose() { }
        }
    }
}
