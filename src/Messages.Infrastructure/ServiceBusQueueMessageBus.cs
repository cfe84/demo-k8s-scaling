using System;
using System.Text;
using System.Threading.Tasks;
using Messages.Domain;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace Messages.Infrastructure
{
    public class ServiceBusQueueMessageBus : IMessageBus
    {
        QueueClient client;
        public ServiceBusQueueMessageBus(string connectionString, string queueName = null)
        {
            if (connectionString.Contains(";EntityPath="))
            {
                var elements = connectionString.Split(new[] { ";EntityPath=" }, StringSplitOptions.RemoveEmptyEntries);

                if (queueName == null && elements.Length != 2)
                    throw new Exception("Missing queue name");
                else
                    queueName = elements[1];
                connectionString = elements[0];
            }
            client = new QueueClient(connectionString, queueName);
        }


        public async Task SendMessageAsync(Domain.Message message)
        {
            var serializedBody = JsonConvert.SerializeObject(message);
            var bodyBytes = Encoding.UTF8.GetBytes(serializedBody);
            var sbMessage = new Microsoft.Azure.ServiceBus.Message(bodyBytes);
            await client.SendAsync(sbMessage);
        }

        public Task StartReceivingAsync(MessageProcessorDelegate OnMessageAsync)
        {
            client.RegisterMessageHandler(async (sbMessage, cancellationToken) =>
            {
                var serializedBody = Encoding.UTF8.GetString(sbMessage.Body);
                var message = JsonConvert.DeserializeObject<Domain.Message>(serializedBody);
                await OnMessageAsync(message);
            }, new MessageHandlerOptions(async (exception) => Console.Error.WriteLine(exception.ToString()))
            {
                AutoComplete = true,
                MaxConcurrentCalls = 1
            });
            return Task.CompletedTask;
        }
    }
}