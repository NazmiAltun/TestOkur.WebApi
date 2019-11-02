namespace TestOkur.Sabit.Integration.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MassTransit.Testing;
    using TestOkur.Contracts.Alert;

    internal class Consumer : MultiTestConsumer
    {
        public Consumer()
            : base(TimeSpan.FromSeconds(10))
        {
            Consume<IUserErrorReceived>();
        }

        public static Consumer Instance { get; } = new Consumer();

        public IEnumerable<T> GetAll<T>()
            where T : class
        {
            return Received.Select<T>().Select(r => r.Context.Message);
        }

        public T GetFirst<T>()
            where T : class
        {
            return Received.Select<T>().First().Context.Message;
        }
    }
}