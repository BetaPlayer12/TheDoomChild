using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Systems;
using DChild.Gameplay.Systems.WorldComponents;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class World : SerializedMonoBehaviour, IGameplaySystemModule, ITime, IWorld
    {
        private float m_timeScale;
        private float m_deltaTime;
        private float m_fixedDeltaTime;

        public float timeScale => m_timeScale;
        public float deltaTime => m_deltaTime;
        public float fixedDeltaTime => m_fixedDeltaTime;

#if UNITY_EDITOR
        public int registeredEnvironmentCount => m_interactiveEnvironmentHandler.registeredObjectCount;
#endif

        private float m_prevTimeScale;
        private TimeIsolationHandler m_timeIsolationHandler;
        [SerializeField]
        private IsolatedPhysicsHandler m_isolatedPhysicsHandler;
        private InteractiveEnvironmentHandler m_interactiveEnvironmentHandler;
        private ShadowEnvironmentHandle m_shadowEnvironmentHandler;
        private bool m_isShadowColliderEnable;

        public float CalculateDeltaTime(float timeScale) => Time.deltaTime * timeScale;
        public float CalculateFixedDeltaTime(float timeScale) => 0.02f * timeScale;

        public void SetTimeScale(float timeScale)
        {
            m_timeScale = timeScale;
            if (m_prevTimeScale != timeScale)
            {
                m_timeIsolationHandler.AlignTimeScale();
                m_prevTimeScale = timeScale;
            }
        }

        public void Register(IFXObjects fx) => m_timeIsolationHandler.Register(fx);
        public void Unregister(IFXObjects fx) => m_timeIsolationHandler.Unregister(fx);
        public void Register(IRendererObjects render) => m_timeIsolationHandler.Register(render);
        public void Unregister(IRendererObjects render) => m_timeIsolationHandler.Unregister(render);
        public void Register(IPhysicObjects physicsObject) => m_timeIsolationHandler.Register(physicsObject);
        public void Unregister(IPhysicObjects physicsObject) => m_timeIsolationHandler.Unregister(physicsObject);
        public void Register(ISpineObjects spineAnimations) => m_timeIsolationHandler.Register(spineAnimations);
        public void Unregister(ISpineObjects spineAnimations) => m_timeIsolationHandler.Unregister(spineAnimations);
        public void Register(IIsolatedObject isolatedObject) => m_timeIsolationHandler.Register(isolatedObject);
        public void Unregister(IIsolatedObject isolatedObject) => m_timeIsolationHandler.Unregister(isolatedObject);
        public void RequestUpdateComponentsOf(IIsolatedObject isolatedObject) => m_timeIsolationHandler.RequestUpdateComponentsOf(isolatedObject);
        public void RequestUpdatePhysicsComponentsOf(IIsolatedObject isolatedObject) => m_timeIsolationHandler.RequestUpdatePhysicsComponentsOf(isolatedObject);
        public void Register(IInteractiveEnvironment isolatedObject) => m_interactiveEnvironmentHandler.Register(isolatedObject);
        public void Unregister(IInteractiveEnvironment isolatedObject) => m_interactiveEnvironmentHandler.Unregister(isolatedObject);
        public void Register(IIsolatedPhysics isolatedPhysics) => m_isolatedPhysicsHandler.Register(isolatedPhysics);
        public void Unregister(IIsolatedPhysics isolatedPhysics) => m_isolatedPhysicsHandler.Unregister(isolatedPhysics);

        public void CleanUp()
        {
            m_timeIsolationHandler.CleanUp();
        }

        public void ResetWorld()
        {
            m_timeScale = m_prevTimeScale = 1;
            m_deltaTime = Time.deltaTime;
            m_fixedDeltaTime = Time.fixedDeltaTime;
            CleanUp();
        }

        public void Register(ShadowEnvironmentHandle handler)
        {
            m_shadowEnvironmentHandler = handler;
            if (m_shadowEnvironmentHandler != null)
            {
                m_shadowEnvironmentHandler.SetCollisions(m_isShadowColliderEnable);
            }
        }

        public void SetShadowColliders(bool enable)
        {
            m_isShadowColliderEnable = enable;
            if (m_shadowEnvironmentHandler != null)
            {
                m_shadowEnvironmentHandler.SetCollisions(enable);
            }
        }

        private void Awake()
        {
            m_timeScale = m_prevTimeScale = 1;
            m_deltaTime = Time.deltaTime;
            m_fixedDeltaTime = Time.fixedDeltaTime;
            m_timeIsolationHandler = new TimeIsolationHandler(timeScale);
            m_interactiveEnvironmentHandler = new InteractiveEnvironmentHandler();
            m_isolatedPhysicsHandler = new IsolatedPhysicsHandler();
        }

        private void FixedUpdate()
        {
            m_isolatedPhysicsHandler.UpdatePhysics();
            m_timeIsolationHandler.FixedUpdate();
        }

        private void Update()
        {
            m_deltaTime = CalculateDeltaTime(timeScale);
            m_timeIsolationHandler.Update(timeScale);
        }

        private void LateUpdate()
        {
            m_interactiveEnvironmentHandler.Update();
        }
    }
}