using System;

namespace Messages.Domain
{
    public class Message
    {
        public DateTime SentDateTime { get; set; }
        public string MessageContent { get; set; }
        public int StayBusySeconds { get; set; }
        public int SequenceNumber { get; set; }
    }
}