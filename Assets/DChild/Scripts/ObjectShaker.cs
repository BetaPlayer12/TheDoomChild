using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild
{
    [AddComponentMenu("DChild/Misc/Object Shaker")]
    public class ObjectShaker : MonoBehaviour
    {
        [System.Serializable]
        private struct ShakeInfo
        {
            private enum TimeType
            {
                Sine,
                Cosine
            }

            [SerializeField]
            private float m_speed;
            [SerializeField]
            private float m_strength;
            [SerializeField]
            private TimeType m_type;

            public float CalculateShakeOffset(float deltaTime)
            {
                if (m_type == TimeType.Sine)
                {
                    return Mathf.Sin(deltaTime * m_speed) * m_strength;
                }
                else
                {
                    return Mathf.Cos(deltaTime * m_speed) * m_strength;
                }
            }
        }

        [BoxGroup("ShakeInfo")]
        [HorizontalGroup("ShakeInfo/Split")]

        [SerializeField, BoxGroup("ShakeInfo/Split/X"), HideLabel]
        private ShakeInfo m_shakeX;
        [SerializeField, BoxGroup("ShakeInfo/Split/Y"), HideLabel]
        private ShakeInfo m_shakeY;
        [SerializeField, HideInPlayMode]
        private bool m_useAnchoredPosition;
        private Vector3 m_originalPosition;
        private RectTransform m_rectTransform;

        private Vector3 position
        {
            get
            {
                return m_useAnchoredPosition ? m_rectTransform.anchoredPosition3D : transform.position;
            }
            set
            {
                if (m_useAnchoredPosition)
                {
                    m_rectTransform.anchoredPosition3D = value;
                }
                else
                {
                    transform.position = value;
                }
            }
        }

        private void Awake()
        {
            if (m_useAnchoredPosition)
            {
                m_rectTransform = GetComponent<RectTransform>();
            }
            m_originalPosition = position;
        }

        private void OnDisable()
        {
            position = m_originalPosition;
        }

        private void LateUpdate()
        {
            var shakedposition = m_originalPosition;
            var deltaTime = Time.time;
            shakedposition.x += m_shakeX.CalculateShakeOffset(deltaTime);
            shakedposition.y += m_shakeY.CalculateShakeOffset(deltaTime);
            position = shakedposition;
        }
    } 
}
