using System.Collections.Generic;
using Framework.Abstractions.Exceptions.CodeGuard.Internals;

namespace Framework.Abstractions.Exceptions.CodeGuard
{
    public interface IArg<T>
    {
        T Value { get; }
        ArgName Name { get; }
        IMessageHandler<T> Message { get; }
        IEnumerable<ErrorInfo> Errors { get; }
    }
}
