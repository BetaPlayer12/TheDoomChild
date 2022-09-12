using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Systems.WorldComponents;

namespace DChild.Gameplay.Systems
{
    public interface IWorld
    {
        void SetTimeScale(float timeScale);
        float CalculateDeltaTime(float timeScale);
        float CalculateFixedDeltaTime(float timeScale);

        void Register(IFXObjects fx);
        void Unregister(IFXObjects fx);
        void Register(IRendererObjects render);
        void Unregister(IRendererObjects render);
        void Register(IPhysicObjects physicsObject);
        void Unregister(IPhysicObjects physicsObject);
        void Register(ISpineObjects spineAnimations);
        void Unregister(ISpineObjects spineAnimations);
        void Register(IIsolatedObject isolatedObject);
        void Unregister(IIsolatedObject isolatedObject);
        void RequestUpdateComponentsOf(IIsolatedObject isolatedObject);
        void RequestUpdatePhysicsComponentsOf(IIsolatedObject isolatedObject);
        void Register(IInteractiveEnvironment isolatedObject);
        void Unregister(IInteractiveEnvironment isolatedObject);
        void Register(IIsolatedPhysics isolatedPhysics);
        void Unregister(IIsolatedPhysics isolatedPhysics);

        void Register(ShadowEnvironmentHandle handler);

        void SetShadowColliders(bool enable);
    }
}