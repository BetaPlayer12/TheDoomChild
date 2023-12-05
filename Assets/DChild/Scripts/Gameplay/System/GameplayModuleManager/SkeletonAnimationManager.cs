using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay
{
    public class SkeletonAnimationManager : GameplayModuleManager<SpineAnimation>, IGameplayUpdateModule, IGameplayLateUpdateModule
    {
        public override string name => "SkeletonAnimationManager";

        private static SkeletonAnimationManager m_instance;
        public static SkeletonAnimationManager Instance => m_instance;

        private Dictionary<SpineAnimation, bool> m_lastSpineStatePair;
        private bool m_isPaused;

        public void PauseAllSpines()
        {
            if (m_lastSpineStatePair == null)
            {
                Debug.Log("Was null");
                m_lastSpineStatePair = new Dictionary<SpineAnimation, bool>();
            }

            if (m_isPaused == false)
            {
                m_lastSpineStatePair.Clear();
            }

            for (int i = 0; i < m_list.Count; i++)
            {
                var animation = m_list[i];
                if (m_lastSpineStatePair.ContainsKey(animation) == false)
                {
                    m_lastSpineStatePair.Add(animation, animation.IsAnimationEnabled());
                    animation.DisableAnimation();
                }
            }
            m_isPaused = true;
        }

        public void UnpauseAllSpines()
        {
            if (m_isPaused == false)
                return;

            if (m_lastSpineStatePair == null)
            {
                Debug.Log("Unpaused Was Null");
                return;
            }

            for (int i = 0; i < m_list.Count; i++)
            {
                var animation = m_list[i];
                if (m_lastSpineStatePair.TryGetValue(animation, out bool enableAnimation))
                {
                    if (enableAnimation == false)
                    {
                        animation.EnableAnimation();
                    }
                }
            }

            m_isPaused = false;
        }
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

        protected override void SetSingletonInstance(IGameplayModuleManager instance)
        {
            m_instance = (SkeletonAnimationManager)instance;
        }
    }
}
