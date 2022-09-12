using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEngine.Profiling;
#endif

namespace DChild.Gameplay
{
    public class GameplayModuleSystem : SerializedMonoBehaviour
    {
        [SerializeField]
        private IGameplayModuleManager[] m_managers;

        private List<IGameplayFixedUpdateModule> m_fixedUpdateModule;
        private List<IGameplayUpdateModule> m_updateModule;
        private List<IGameplayLateUpdateModule> m_lateUpdateModule;

#if UNITY_EDITOR
        private const string PROFILER_PREFIX = "GameplayModuleSystem/";
#endif

        private void Awake()
        {
            var fixedUpdateType = typeof(IGameplayFixedUpdateModule);
            var updateType = typeof(IGameplayUpdateModule);
            var lateUpdateType = typeof(IGameplayLateUpdateModule);

            m_fixedUpdateModule = new List<IGameplayFixedUpdateModule>();
            m_updateModule = new List<IGameplayUpdateModule>();
            m_lateUpdateModule = new List<IGameplayLateUpdateModule>();

            for (int i = 0; i < m_managers.Length; i++)
            {
                var manager = m_managers[i];
                manager.SetInstance(manager);
                var managerType = manager.GetType();
                if (fixedUpdateType.IsAssignableFrom(managerType))
                {
                    m_fixedUpdateModule.Add((IGameplayFixedUpdateModule)manager);
                }

                if (updateType.IsAssignableFrom(managerType))
                {
                    m_updateModule.Add((IGameplayUpdateModule)manager);
                }

                if (lateUpdateType.IsAssignableFrom(managerType))
                {
                    m_lateUpdateModule.Add((IGameplayLateUpdateModule)manager);
                }
            }
        }

        private void FixedUpdate()
        {
            var deltaTime = GameplaySystem.time.fixedDeltaTime;
            for (int i = 0; i < m_fixedUpdateModule.Count; i++)
            {
#if UNITY_EDITOR
                Profiler.BeginSample(PROFILER_PREFIX + m_fixedUpdateModule[i].name);
#endif
                m_fixedUpdateModule[i].FixedUpdateModule(deltaTime);
#if UNITY_EDITOR
                Profiler.EndSample();
#endif
            }
        }

        private void Update()
        {
            var deltaTime = GameplaySystem.time.deltaTime;
            for (int i = 0; i < m_updateModule.Count; i++)
            {
#if UNITY_EDITOR
                Profiler.BeginSample(PROFILER_PREFIX + m_updateModule[i].name);
#endif
                m_updateModule[i].UpdateModule(deltaTime);
#if UNITY_EDITOR
                Profiler.EndSample();
#endif
            }
        }

        private void LateUpdate()
        {
            var deltaTime = GameplaySystem.time.deltaTime;
            for (int i = 0; i < m_lateUpdateModule.Count; i++)
            {
#if UNITY_EDITOR
                Profiler.BeginSample(PROFILER_PREFIX + m_lateUpdateModule[i].name);
#endif
                m_lateUpdateModule[i].LateUpdateModule(deltaTime);
#if UNITY_EDITOR
                Profiler.EndSample();
#endif
            }
        }
    }
}
