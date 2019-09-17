namespace TestOkur.Report.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.Extensions.Logging;

    public class DefaultFaultConsumer : IConsumer<Fault>
    {
        private readonly ILogger<DefaultFaultConsumer> _logger;

        public DefaultFaultConsumer(ILogger<DefaultFaultConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<Fault> context)
        {
            foreach (var exception in context.Message.Exceptions)
            {
                _logger.LogError($"{exception.ExceptionType} : {exception.Source} ==>" +
                                 $" {exception.Message} : {exception.StackTrace}");
            }

            return Task.CompletedTask;
        }
    }
}
