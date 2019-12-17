namespace TestOkur.Notification.Consumers
{
    using MassTransit;
    using System.Threading.Tasks;
    using TestOkur.Contracts.App;
    using TestOkur.Notification.Configuration;
    using TestOkur.Notification.Infrastructure.Data;
    using TestOkur.Notification.Models;

    public class CommandQueryLogEventConsumer : IConsumer<ICommandQueryLogEvent>
    {
        private readonly TestOkurContext _context;

        public CommandQueryLogEventConsumer(ApplicationConfiguration configuration)
        {
            _context = new TestOkurContext(configuration);
        }

        public Task Consume(ConsumeContext<ICommandQueryLogEvent> context)
        {
            return _context.CommandQueryLogs.InsertOneAsync(new CommandQueryLog(context.Message));
        }
    }
}
