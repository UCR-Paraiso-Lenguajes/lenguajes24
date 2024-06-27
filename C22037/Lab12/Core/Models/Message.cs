using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace TodoApi
{
    public sealed class Message
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }

    }
}