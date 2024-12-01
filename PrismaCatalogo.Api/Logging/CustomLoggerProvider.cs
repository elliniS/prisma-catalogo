using System.Collections.Concurrent;

namespace PrismaCatalogo.Api.Logging
{
    public class CustomLoggerProvider : ILoggerProvider
    {
        readonly CustomLoggerProviderConfiguration config;

        readonly ConcurrentDictionary<string, CustomerLogger> logger = new ConcurrentDictionary<string, CustomerLogger>();

        public CustomLoggerProvider(CustomLoggerProviderConfiguration config)
        {
            this.config = config;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return logger.GetOrAdd(categoryName, n => new CustomerLogger(n,  config));
        }

        public void Dispose()
        {
            logger.Clear();
        }
    }
}
