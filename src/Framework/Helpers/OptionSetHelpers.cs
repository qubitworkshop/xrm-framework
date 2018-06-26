using System;
using Microsoft.Xrm.Sdk;

namespace Qubit.Xrm.Framework.Helpers
{
    public static class OptionSetHelpers
    {
        public static OptionSetValue ToOptionSetValue(this Enum enumValue)
        {
            return new OptionSetValue(Convert.ToInt32(enumValue));
        }

        public static T ToEnum<T>(this OptionSetValue optionSetValue)
        {
            return optionSetValue == null
                ? (T)Enum.ToObject(typeof(T), default(int))
                : (T)Enum.ToObject(typeof(T), optionSetValue.Value);
        }
    }
}
