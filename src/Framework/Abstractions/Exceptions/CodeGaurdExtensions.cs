using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seterlund.CodeGuard;

namespace Qubit.Xrm.Framework.Abstractions.Exceptions
{
    public static class CodeGaurdExtensions
    {
        public static IArg<T> WithExceptions<T>(this IArg<T> arg, Action<T, IEnumerable<ErrorInfo>> handleErrorsAction) where T : IComparable
        {
            if (arg.Errors != null && arg.Errors.Any())
            {
                handleErrorsAction(arg.Value, arg.Errors);
            }

            return arg;
        }
    }
}
