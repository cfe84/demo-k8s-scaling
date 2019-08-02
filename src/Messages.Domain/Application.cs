using System;
using System.Threading;
using System.Threading.Tasks;

namespace Messages.Domain
{
    public class Application
    {
        IMessageBus messageBus;

        public Application(IMessageBus messageBus)
        {
            this.messageBus = messageBus;
        }

        public async Task SendMessageAsync(Message message)
        {
            await messageBus.SendMessageAsync(message);
        }

        private Task ReceiveMessageAsync(Message message)
        {
            Console.WriteLine($"#{message.SequenceNumber}: {message.MessageContent}. Staying busy {message.StayBusySeconds}s");
            Thread.Sleep(TimeSpan.FromSeconds(message.StayBusySeconds));
            return Task.CompletedTask;
        }

        public async Task StartAsync()
        {
            await messageBus.StartReceivingAsync(ReceiveMessageAsync);
        }
    }
}