using System;

namespace Qubit.Xrm.Framework.Core
{
    /// <summary>
    /// Provides the framework with information on which plugin messages to execute 
    /// this plugin on
    /// </summary>
    public class MessagesAttribute : Attribute
    {
        public string[] Messages { get; }

        public MessagesAttribute(params string[] messages)
        {
            Messages = messages;
        }
    }
}
