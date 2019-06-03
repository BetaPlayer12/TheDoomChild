using DChild.Gameplay.Systems.WorldComponents;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class TimeIsolationHandler
    {
        private bool m_alignTimeScale;
        private ParticleTimeHandler m_particleHandler;
        private RendererTimeHandler m_rendererHandler;
        private PhysicsTimeHandler m_physicsHandler;
        private SpineTimeHandler m_spineHandler;
        private IsolatedObjectHandler m_isolatedObjectHandler;

        public TimeIsolationHandler(float timeScale)
        {
            m_alignTimeScale = false;
            m_particleHandler = new ParticleTimeHandler(timeScale);
            m_rendererHandler = new RendererTimeHandler(timeScale);
            m_physicsHandler = new PhysicsTimeHandler(timeScale);
            m_spineHandler = new SpineTimeHandler(timeScale);
            m_isolatedObjectHandler = new IsolatedObjectHandler(timeScale);
        }

        public float CalculateDeltaTime(float timeScale) => Time.deltaTime * timeScale;
        public float CalculateFixedDeltaTime(float timeScale) => 0.02f * timeScale;

        public void AlignTimeScale()
        {
            m_alignTimeScale = true;
        }

        public void Register(IFXObjects fx) => m_particleHandler.Register(fx);
        public void Unregister(IFXObjects fx) => m_particleHandler.Unregister(fx);
        public void Register(IRendererObjects render) => m_rendererHandler.Register(render);
        public void Unregister(IRendererObjects render) => m_rendererHandler.Unregister(render);
        public void Register(IPhysicObjects physicsObject) => m_physicsHandler.Register(physicsObject);
        public void Unregister(IPhysicObjects physicsObject) => m_physicsHandler.Unregister(physicsObject);
        public void Register(ISpineObjects spineAnimations) => m_spineHandler.Register(spineAnimations);
        public void Unregister(ISpineObjects spineAnimations) => m_spineHandler.Unregister(spineAnimations);
        public void Register(IIsolatedObject isolatedObject) => m_isolatedObjectHandler.Register(isolatedObject);
        public void Unregister(IIsolatedObject isolatedObject) => m_isolatedObjectHandler.Unregister(isolatedObject);
        public void RequestUpdateComponentsOf(IIsolatedObject isolatedObject) => m_isolatedObjectHandler.UpdateComponentOf(isolatedObject);
        public void RequestUpdatePhysicsComponentsOf(IIsolatedObject isolatedObject) => m_isolatedObjectHandler.UpdatePhysicsComponentOf(isolatedObject);

        public void CleanUp()
        {
            m_particleHandler.ClearNull();
            m_rendererHandler.ClearNull();
            m_physicsHandler.ClearNull();
            m_spineHandler.ClearNull();
            m_isolatedObjectHandler.ClearNull();
        }

        public void Reset()
        {
            m_alignTimeScale = true;
            CleanUp();
        }

        public void FixedUpdate()
        {
            m_physicsHandler.CalculateActualVelocity();
            m_isolatedObjectHandler.CalculateActualVelocity();
            m_isolatedObjectHandler.UpdatePhysicsComponents();
        }

        public void Update(float timeScale)
        {
            m_isolatedObjectHandler.UpdateComponents();
            if (m_alignTimeScale)
            {
                m_particleHandler.AlignTime(timeScale);
                m_rendererHandler.AlignTime(timeScale);
                m_physicsHandler.AlignTime(timeScale);
                m_spineHandler.AlignTime(timeScale);
                m_isolatedObjectHandler.AlignTime(timeScale);
                m_alignTimeScale = false;
            }
            m_isolatedObjectHandler.UpdateDeltaTime();
        }
    }
}