namespace Qubit.Xrm.Framework.Core.Plugins
{
    /// <summary>
    /// CRM plugin pipeline stages
    /// </summary>
    public enum PipelineStages
    {
        Invalid = 0,
        PreValidation = 10,
        PreOperation = 20,
        PostOperation = 40
    }
}
