using Holysoft.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Holysoft.UI
{
    public abstract class RectLerpAnimation : UIAnimation
    {
        [SerializeField]
        protected RectTransform m_target;
        [SerializeField]
        [PropertyOrder(1)]
        protected LerpDuration m_lerp;
        [SerializeField]
        [PropertyOrder(1)]
        protected Vector3 m_fromValue;
        [SerializeField]
        [PropertyOrder(1)]
        protected Vector3 m_toValue;
        protected bool m_lerpToValue;

        protected abstract Vector3 target { get; set; }

        public override void Play()
        {
            enabled = true;
            target = m_fromValue;
        }

#if UNITY_EDITOR
        [Button("Copy To From Value")]
        [PropertyOrder(200)]
        private void CopyValueToFromValue() => m_fromValue = target;

        [Button("Copy To To Value")]
        [PropertyOrder(200)]
        private void CopyValueToFrom() => m_toValue = target;

        [Button("Use From Value")]
        [PropertyOrder(200)]
        private void UseFromValue() => target = m_fromValue;

        [Button("Use To Value")]
        [PropertyOrder(200)]
        private void UseToValue() => target = m_toValue;
#endif

        private void OnValidate()
        {
            if (m_target == null)
            {
                m_target = GetComponent<RectTransform>();
                m_fromValue = target;
                m_toValue = target;
            }
        }
    }

}