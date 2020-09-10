using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    [ExecuteAlways]
    public class TargetMarker : MonoBehaviour
    {
        [SerializeField, OnValueChanged("UpdatePosition")]
        private Transform m_target;
        [SerializeField, OnValueChanged("UpdatePosition")]
        private Vector2 m_targetOrigin;
        [SerializeField, HorizontalGroup("Anchor"), OnValueChanged("UpdatePosition")]
        private Vector2 m_anchorOrigin;
        [SerializeField, MinValue(0.1f), OnValueChanged("UpdatePosition")]
        private float m_toUIScale = 1;

        private RectTransform m_rectTransform;
        private Vector3 m_prevTargetPosition;

        public void SetTrackingInfo(Vector2 targetOrigin, Vector2 anchorOrigin, float toUIScale)
        {
            m_targetOrigin = targetOrigin;
            m_anchorOrigin = anchorOrigin;
            m_toUIScale = toUIScale;
            UpdateMarkerPosition(m_target.position);
        }

        private void UpdateMarkerPosition(Vector2 position)
        {
            var travelPosition = (position - m_targetOrigin) * m_toUIScale;
            m_rectTransform.anchoredPosition = m_anchorOrigin + travelPosition;
        }

        private void UpdatePosition()
        {
            var position = m_target.position;
            UpdateMarkerPosition(position);
            m_prevTargetPosition = position;
        }

        private void Start()
        {
            m_rectTransform = GetComponent<RectTransform>();
            UpdatePosition();
        }

        private void OnEnable()
        {
            UpdatePosition();
        }

        private void LateUpdate()
        {
#if UNITY_EDITOR
            if (m_target == null)
                return;
#endif

            var position = m_target.position;
            if (position != m_prevTargetPosition)
            {
                UpdateMarkerPosition(position);
                m_prevTargetPosition = position;
            }
        }

#if UNITY_EDITOR
        [ButtonGroup("Anchor/Button"), Button("UseCurrent")]
        private void UseCurrentAnchorPosition()
        {
            m_anchorOrigin = GetComponent<RectTransform>().anchoredPosition;
        }
#endif
    }
}