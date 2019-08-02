using System;
using System.Threading;
using System.Threading.Tasks;
using Messages.Domain;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace Messages.Infrastructure
{
    public class StorageQueueMessageBus : IMessageBus
    {
        CloudQueue queue;

        public StorageQueueMessageBus(string connectionString, string queueName)
        {
            var account = CloudStorageAccount.Parse(connectionString);
            var queueClient = account.CreateCloudQueueClient();
            queue = queueClient.GetQueueReference(queueName);
        }

        public async Task SendMessageAsync(Message message)
        {
            var content = JsonConvert.SerializeObject(message);
            var queueMessage = new CloudQueueMessage(content);
            await queue.AddMessageAsync(queueMessage);
        }

        public async Task StartReceivingAsync(MessageProcessorDelegate OnMessageAsync)
        {
            while (true)
            {
                var queueMessage = await queue.GetMessageAsync();
                if (queueMessage != null)
                {
                    var message = JsonConvert.DeserializeObject<Message>(queueMessage.AsString);
                    await OnMessageAsync(message);
                    await queue.DeleteMessageAsync(queueMessage);
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
