using System;

namespace Diploma.Client.Network.Request
{
    public class CreateChatRequest
    {
        public string Name { get; set; }
        public Guid UserId { get; set; }
    }
}