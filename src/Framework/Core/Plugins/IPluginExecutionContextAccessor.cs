namespace Framework.Core.Plugins
{
    public interface IPluginExecutionContextAccessor : IExecutionContextAccessor
    {
        PipelineStages Stage { get; }

        string MessageName { get; }
    }
}
