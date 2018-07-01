using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qubit.Xrm.Framework.Mock.Core
{
    public class EmbeddedResourceNotFoundException : Exception
    {
        public EmbeddedResourceNotFoundException(string resourceName)
            : base($"'{resourceName}' was not found. Please make sure the resource exists and that it is marked as an embedded resource in the project")
        { }
    }
}
