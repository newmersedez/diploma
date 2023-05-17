using System;

namespace Diploma.Client.Network.Request
{
    public class CreateMessageRequest
    {
        public Guid? AttachmentId { get; set; }
        public string Text { get; set; }
    }
}