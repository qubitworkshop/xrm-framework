using System;

namespace Qubit.Xrm.Framework.Core
{
    /// <summary>
    /// Provides api to access context information
    /// </summary>
    public interface IExecutionContextAccessor
    {
        /// <summary>
        /// Provides information about the target context entity in the current execution context
        /// </summary>
        Target Target { get; }

        /// <summary>
        /// Information about the context user
        /// </summary>
        User User { get; }

        /// <summary>
        /// The CRM execution context correlation ID
        /// </summary>
        Guid CorrelationId { get; }

        /// <summary>
        /// The CRM execution context operation ID
        /// </summary>
        Guid OperationId { get; }

        /// <summary>
        /// Handle exceptions through the framework's exception handler. This will automatically log the exception 
        /// and bubble up the exception to the parent handle
        /// </summary>
        /// <param name="ex">The exception to log</param>
        void HandleException(Exception ex);
    }
}
