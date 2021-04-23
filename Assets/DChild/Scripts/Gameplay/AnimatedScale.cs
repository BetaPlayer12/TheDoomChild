using DChild.Gameplay;
using UnityEngine;

namespace DChild.Gameplay
{
    public class AnimatedScale : MonoBehaviour
    {
        [SerializeField]
        private Vector3 m_from;
        [SerializeField]
        private Vector3 m_to;
        [SerializeField]
        private AnimationCurve m_lerpInfo;

        private float m_timer;

        private void AdjustScale()
        {
            var lerp = m_lerpInfo.Evaluate(m_timer);
            var scale = Vector3.Lerp(m_from, m_to, lerp);
            transform.localScale = scale;
        }

        private void OnEnable()
        {
            m_timer = 0;
            AdjustScale();
        }

        private void LateUpdate()
        {
            m_timer += GameplaySystem.time.deltaTime;
            AdjustScale();
        }
    }
}