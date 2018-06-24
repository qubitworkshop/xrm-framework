using System;

namespace Qubit.Xrm.Framework.Core
{
    public class InvalidPipelineExecutionException : Exception
    {
        public InvalidPipelineExecutionException(string message) : base(message)
        { }
    }
}
