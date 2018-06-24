using Ninject;
using Ninject.Infrastructure.Disposal;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Abstractions.DependencyInjection
{
    public interface IDependencyResolver : IBindingRoot, IResolutionRoot, IServiceProvider, IDisposableObject, INotifyWhenDisposed
    { }
}
