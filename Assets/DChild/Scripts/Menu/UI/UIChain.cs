using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.UI
{
    public class UIChain : MonoBehaviour
    {
        [SerializeField]
        private Canvas m_canvas;
        [SerializeField]
        private RectTransform m_from;
        [SerializeField]
        private Vector3 m_fromOffset;
        [SerializeField]
        private RectTransform m_to;
        [SerializeField]
        private float m_lengthOffset;

        private RectTransform m_rectTransform;
        [SerializeField, ReadOnly]
        private Vector3 m_lastFromPosition;
        [SerializeField, ReadOnly]
        private Vector3 m_lastToPosition;

        [ContextMenu("Update Chain")]
        private void UpdateChain()
        {
            if (m_rectTransform == null)
            {
                Start();
            }

            var position = m_from.position + m_fromOffset;

            transform.position = position;

            var direction = m_canvas.transform.InverseTransformVector(m_to.position) - m_canvas.transform.InverseTransformVector(position);
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            var sizeDelta = m_rectTransform.sizeDelta;
            sizeDelta.x = (direction.magnitude + m_lengthOffset);

            m_rectTransform.sizeDelta = sizeDelta;

            m_lastFromPosition = position;
            m_lastToPosition = m_to.position;
        }

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
                UpdateChain();
            }
        }

    }

}