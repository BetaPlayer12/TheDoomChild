using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Environment
{
    public class AnimatedLightAdjusterHandle : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve m_animation;
        [SerializeField]
        private bool m_playOnStart = true;
        [SerializeField]
        private bool m_loop;
        [SerializeField, MinValue(0.1f)]
        private float m_duration;
        [SerializeField]
        private LightIntensityAdjuster[] m_lightAdjusters;

        private float m_animationTime;

        public void StartAnimation()
        {
            m_animationTime = 0;
            enabled = true;
        }

        public void PauseAnimation()
        {
            enabled = false;
        }

        private void Awake()
        {
            m_animationTime = 0;
            enabled = m_playOnStart;
        }

        private void LateUpdate()
        {
            m_animationTime += GameplaySystem.time.deltaTime;
            foreach (var lightAdjuster in m_lightAdjusters)
            {
                lightAdjuster.SetIntensity(m_animation.Evaluate(m_animationTime));
            }

            if (m_loop == false)
            {
                if (m_animationTime > m_duration)
                {
                    enabled = false;
                }
            }
        }
    }
}
