using System;

namespace Qubit.Xrm.Framework.Core
{
    public interface IExecutionContextAccessor
    {
        Target Target { get; }
        User User { get; }

        Guid CorrelationId { get; }
        Guid OperationId { get; }
        void HandleException(Exception ex);

        void ValidateExecution(Type implementationType);
    }
}
