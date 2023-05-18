using System;

namespace Diploma.Client.Network.Request
{
    public class CreateMessageRequest
    {
        public Guid? FileId { get; set; }
        public string Text { get; set; }
    }
}