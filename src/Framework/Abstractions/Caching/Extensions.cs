namespace Qubit.Xrm.Framework.Abstractions.Caching
{
    public static class Extensions
    {
        public static string GetCacheKey<TService>(this string key)
        {
            return $"{nameof(TService)}:{key}";
        }
    }
}
