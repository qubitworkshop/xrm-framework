using System;

namespace Qubit.Xrm.Framework.Core
{
    /// <summary>
    /// This exception is thrown if a pipeline plugin encouters an execution failure that is unhandled
    /// </summary>
    public class InvalidPipelineExecutionException : Exception
    {
        public InvalidPipelineExecutionException(string message) : base(message)
        { }
    }
}
