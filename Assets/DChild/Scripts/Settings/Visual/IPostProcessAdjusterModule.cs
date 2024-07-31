using UnityEngine.Rendering;

namespace DChild.Configurations.Visuals
{
    public interface IPostProcessAdjusterModule
    {
        void ApplyConfiguration(PostProcessConfiguration configuration);

        void ModifyConfiguration(ref PostProcessConfiguration configuration);

        bool ValidatePostProcess(Volume volume, ref string message);
    }
}
