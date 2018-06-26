namespace Qubit.Xrm.Framework.Abstractions.Configuration
{
    public interface ISettingsProvider
    {
        string Get(string key);
        T Get<T>(string key) where T : class;
    }
}
