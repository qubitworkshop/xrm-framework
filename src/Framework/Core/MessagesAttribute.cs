using System;

namespace Framework.Core
{
    public class MessagesAttribute : Attribute
    {
        public string[] Messages { get; }

        public MessagesAttribute(params string[] messages)
        {
            Messages = messages;
        }
    }
}
