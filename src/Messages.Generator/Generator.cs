using System;
using System.Threading;
using System.Threading.Tasks;
using Messages.Domain;
using Messages.Infrastructure;

namespace Messages.Generator
{
    class Generator
    {
        Application application;
        int loopDelayMilliseconds;
        int busyTimeSeconds;
        public Generator(IMessageBus messageBus, int loopDelayMilliseconds = 100, int busyTimeSeconds = 1)
        {
            this.loopDelayMilliseconds = loopDelayMilliseconds;
            this.busyTimeSeconds = busyTimeSeconds;
            application = new Application(messageBus);
        }

        public async Task SendLoopAsync()
        {
            int sequenceNumber = 0;
            while (true)
            {
                var message = new Message
                {
                    SequenceNumber = ++sequenceNumber,
                    MessageContent = "This is the message",
                    SentDateTime = DateTime.Now,
                    StayBusySeconds = busyTimeSeconds
                };
                Console.WriteLine($"Sending message {sequenceNumber}");
                await application.SendMessageAsync(message);
                Thread.Sleep(TimeSpan.FromMilliseconds(loopDelayMilliseconds));
            }
        }
    }
}