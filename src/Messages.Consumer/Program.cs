using System;
using System.Threading;
using Messages.Domain;
using Messages.Infrastructure;

namespace Messages.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = EnvironmentMessageBusFactory.GetMessageBus();
            var application = new Application(bus);
            application.StartAsync().Wait();
            Thread.Sleep(-1);
        }
    }
}
