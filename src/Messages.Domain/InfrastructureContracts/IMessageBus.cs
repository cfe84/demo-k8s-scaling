using System.Threading.Tasks;

namespace Messages.Domain
{
    public delegate Task MessageProcessorDelegate(Message message);

    public interface IMessageBus
    {
        Task SendMessageAsync(Message message);
        Task StartReceivingAsync(MessageProcessorDelegate OnMessageAsync);
    }
}