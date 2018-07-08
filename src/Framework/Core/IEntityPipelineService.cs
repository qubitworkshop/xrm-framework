namespace Qubit.Xrm.Framework.Core
{
    public interface IEntityPipelineService
    {
        /// <summary>
        /// Handles the pipeline event fired by CRM's platform
        /// </summary>
        void HandlePipelineEvent();
    }
}
