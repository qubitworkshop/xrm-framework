﻿using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Seterlund.CodeGuard;

namespace Qubit.Xrm.Framework.Mock.Core
{
    public static class AssemblyExtensions
    {
        public static string ReadEmbeddedResourceAsString(this Assembly assembly, string resource)
        {
            using (Stream stream = assembly.GetManifestResourceStream(resource))
            {
                //Validate.That(() => stream).IsNotNull()
                //    .WithExceptions((value, errors) => throw new EmbeddedResourceNotFoundException(resource));

                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static T GetEmbeddedJsonResource<T>(this Assembly assembly, string resource)
        {
            string json = assembly.ReadEmbeddedResourceAsString(resource);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
