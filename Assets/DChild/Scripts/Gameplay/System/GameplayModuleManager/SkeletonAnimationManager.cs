using Spine.Unity;

namespace DChild.Gameplay
{
    public class SkeletonAnimationManager : GameplayModuleManager<SpineAnimation>, IGameplayUpdateModule, IGameplayLateUpdateModule
    {
        public override string name => "SkeletonAnimationManager";

        public void LateUpdateModule(float deltaTime)
        {
            for (int i = 0; i < m_list.Count; i++)
            {
                m_list[i].LateUpdateAnimation();
            }

        }

        public void UpdateModule(float deltaTime)
        {
            for (int i = 0; i < m_list.Count; i++)
            {
                m_list[i].UpdateAnimation(deltaTime);
            }
        }
    }
}
