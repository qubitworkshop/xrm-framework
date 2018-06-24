namespace Framework.Core.CodeActivities
{
    public interface ICodeActivityExecutionContextAccessor : IExecutionContextAccessor
    {
        string StageName { get; }
    }
}
