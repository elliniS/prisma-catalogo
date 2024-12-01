
namespace PrismaCatalogo.Api.Logging
{
    public class CustomerLogger : ILogger
    {
        readonly string _name;
        readonly CustomLoggerProviderConfiguration _configuration;

        public CustomerLogger(string name, CustomLoggerProviderConfiguration configuration)
        {
            this._name = name;
            this._configuration = configuration;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == _configuration.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string mensagem = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";

            EscreverTextoNoArquivo(mensagem);
        }

        private void EscreverTextoNoArquivo(string mensagem)
        {
            try
            {
                string caminhoArquivoLog = @"CAMINHO_ARQUIVO_LOG";
                using (StreamWriter streamWriter = new StreamWriter(caminhoArquivoLog, true))
                {
                    try
                    {
                        streamWriter.WriteLine(mensagem);
                        streamWriter.Close();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch
            {

            }
        }
    }
}
