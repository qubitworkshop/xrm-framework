using System;

namespace Framework.Core
{
    public class InvalidPipelineExecutionException : Exception
    {
        public InvalidPipelineExecutionException(string message) : base(message)
        { }
    }
}
