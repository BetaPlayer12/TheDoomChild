using UnityEngine;

namespace DChild.Gameplay
{
    public class AnimationCurveMovement : MonoBehaviour
    {
        [SerializeField, Min(0)]
        private float m_amplitude;
        [SerializeField, Min(0)]
        private float m_frequency;
        [SerializeField]
        private AnimationCurve m_curve;

        private float m_time;

        private void OnEnable()
        {
            m_time = 0;
        }

        private void Update()
        {
            m_time += GameplaySystem.time.deltaTime * m_frequency;
            var offset = transform.up * m_curve.Evaluate(m_time) * m_amplitude;
            transform.localPosition = offset;
        }
    }
}