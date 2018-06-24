using System;
using System.Linq.Expressions;
using Framework.Abstractions.Exceptions.CodeGuard.Internals;

namespace Framework.Abstractions.Exceptions.CodeGuard
{
    public static class Validate
    {
        /// <summary>
        /// Validate the argument 
        /// </summary>
        /// <param name="argument">
        /// The argument.
        /// </param>
        /// <typeparam name="T">
        /// Type of the argument
        /// </typeparam>
        /// <returns>
        /// An ArgumentValidator
        /// </returns>
        public static IArg<T> That<T>(Expression<Func<T>> argument)
        {
            return new AccumulateErrorsArg<T>(argument);
        }
    }
}
