using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.UI.Map
{

    [System.Serializable]
    public class MapMover
    {
        [SerializeField]
        private RectTransform m_content;
        [SerializeField, MinValue(0)]
        private float m_speed = 0.1f;

        public void Update()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            if (horizontal != 0 || vertical != 0)
            {
                var deltaTime = Time.unscaledDeltaTime;
                var anchoredPosition = m_content.anchoredPosition;
                anchoredPosition.x -= m_speed * horizontal * deltaTime;
                anchoredPosition.y -= m_speed * vertical * deltaTime;
                m_content.anchoredPosition = anchoredPosition;
            }
        }
    }
}