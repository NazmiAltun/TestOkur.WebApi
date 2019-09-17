namespace TestOkur.Report.Integration.Tests.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MassTransit.Testing;
    using TestOkur.Report.Events;

    internal class Consumer : MultiTestConsumer
    {
        public Consumer()
            : base(TimeSpan.FromSeconds(10))
        {
            Consume<IEvaluateExam>();
        }

        public static Consumer Instance { get; } = new Consumer();

        public IEnumerable<T> GetAll<T>()
            where T : class
        {
            return Received.Select<T>().Select(r => r.Context.Message);
        }
    }
}
