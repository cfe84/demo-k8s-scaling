using System;
using Messages.Domain;

namespace Messages.Infrastructure
{
    public class EnvironmentMessageBusFactory
    {
        private static IMessageBus GetServiceBusMessageBus()
        {
            string CONNECTION_STRING = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            string QUEUE_NAME = Environment.GetEnvironmentVariable("QUEUE_NAME");
            var bus = new ServiceBusQueueMessageBus(CONNECTION_STRING, QUEUE_NAME);
            return bus;
        }

        private static IMessageBus GetStorageQueueServiceBus()
        {
            string CONNECTION_STRING = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            string QUEUE_NAME = Environment.GetEnvironmentVariable("QUEUE_NAME");
            var queue = new StorageQueueMessageBus(CONNECTION_STRING, QUEUE_NAME);
            return queue;
        }

        public static IMessageBus GetMessageBus()
        {
            var MESSAGE_BUS_TYPE = Environment.GetEnvironmentVariable("MESSAGE_BUS_TYPE");
            if (string.Equals("ServiceBus", MESSAGE_BUS_TYPE, StringComparison.CurrentCultureIgnoreCase))
            {
                return GetServiceBusMessageBus();
            }
            else
            {
                return GetStorageQueueServiceBus();
            }
        }
    }
}