using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.UI
{
    [ExecuteInEditMode]
    public class UIChain : MonoBehaviour
    {
        [SerializeField]
        private Transform m_from;
        [SerializeField]
        private Vector3 m_fromOffset;
        [SerializeField]
        private Transform m_to;
        [SerializeField]
        private float m_lengthOffset;

        private RectTransform m_rectTransform;
        [SerializeField, ReadOnly]
        private Vector3 m_lastFromPosition;
        [SerializeField, ReadOnly]
        private Vector3 m_lastToPosition;

        private void Start()
        {
            m_rectTransform = GetComponent<RectTransform>();
            m_lastFromPosition = m_from.position + m_fromOffset;
            m_lastToPosition = m_to.position;
        }

        void Update()
        {
            var position = m_from.position + m_fromOffset;
            if (m_lastFromPosition != position || m_lastToPosition != m_to.position)
            {
                transform.position = position;

                var direction = m_to.position - position;
                var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                var sizeDelta = m_rectTransform.sizeDelta;
                sizeDelta.x = (direction.magnitude + m_lengthOffset) * 10f;

                m_rectTransform.sizeDelta = sizeDelta;

                m_lastFromPosition = position;
                m_lastToPosition = m_to.position;
            }
        }
    }

}