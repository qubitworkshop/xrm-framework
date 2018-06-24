using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Activation;
using Ninject.Parameters;
using Ninject.Planning.Bindings;
using Ninject.Syntax;

namespace Framework.Abstractions.DependencyInjection
{
    public class DependencyResolver : IDependencyResolver
    {
        public bool IsDisposed => throw new NotImplementedException();

        public event EventHandler Disposed;

        public void AddBinding(IBinding binding)
        {
            throw new NotImplementedException();
        }

        public IBindingToSyntax<T> Bind<T>()
        {
            throw new NotImplementedException();
        }

        public IBindingToSyntax<T1, T2> Bind<T1, T2>()
        {
            throw new NotImplementedException();
        }

        public IBindingToSyntax<T1, T2, T3> Bind<T1, T2, T3>()
        {
            throw new NotImplementedException();
        }

        public IBindingToSyntax<T1, T2, T3, T4> Bind<T1, T2, T3, T4>()
        {
            throw new NotImplementedException();
        }

        public IBindingToSyntax<object> Bind(params Type[] services)
        {
            throw new NotImplementedException();
        }

        public bool CanResolve(IRequest request)
        {
            throw new NotImplementedException();
        }

        public bool CanResolve(IRequest request, bool ignoreImplicitBindings)
        {
            throw new NotImplementedException();
        }

        public IRequest CreateRequest(Type service, Func<IBindingMetadata, bool> constraint, IEnumerable<IParameter> parameters, bool isOptional, bool isUnique)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public void Inject(object instance, params IParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public IBindingToSyntax<T1> Rebind<T1>()
        {
            throw new NotImplementedException();
        }

        public IBindingToSyntax<T1, T2> Rebind<T1, T2>()
        {
            throw new NotImplementedException();
        }

        public IBindingToSyntax<T1, T2, T3> Rebind<T1, T2, T3>()
        {
            throw new NotImplementedException();
        }

        public IBindingToSyntax<T1, T2, T3, T4> Rebind<T1, T2, T3, T4>()
        {
            throw new NotImplementedException();
        }

        public IBindingToSyntax<object> Rebind(params Type[] services)
        {
            throw new NotImplementedException();
        }

        public bool Release(object instance)
        {
            throw new NotImplementedException();
        }

        public void RemoveBinding(IBinding binding)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> Resolve(IRequest request)
        {
            throw new NotImplementedException();
        }

        public void Unbind<T>()
        {
            throw new NotImplementedException();
        }

        public void Unbind(Type service)
        {
            throw new NotImplementedException();
        }
    }
}
