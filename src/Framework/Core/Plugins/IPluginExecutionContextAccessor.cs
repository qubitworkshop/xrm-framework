namespace Qubit.Xrm.Framework.Core.Plugins
{
    public interface IPluginExecutionContextAccessor : IExecutionContextAccessor
    {
        /// <summary>
        /// This execution context's pipeline stage
        /// </summary>
        PipelineStages Stage { get; }

        /// <summary>
        /// The execution context's message
        /// </summary>
        string MessageName { get; }
    }
}
