using DChild.Gameplay.Characters.Enemies;
using Holysoft.Event;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DChild.Gameplay.UI.Map
{
    public class MapZoomEventActionArgs : IEventActionArgs
    {
        private Vector2 m_scrollWheel;
        public Vector2 scrollWheel => m_scrollWheel;
        
        private float m_iconScale;
        public float iconScaleRate => m_iconScale;

        public MapZoomEventActionArgs(Vector2 scrollWheel, float iconScaleRate)
        {
            this.m_scrollWheel = scrollWheel;
            this.m_iconScale = iconScaleRate;
        }
    }

    public class MapZoomHandler : MonoBehaviour
    {
        private RectTransform m_currentMap;

        private const float DEFAULT_ZOOM = 1.0f;
        private float m_minZoom;
        private float m_maxZoom;

        private float m_currentY;

        [SerializeField] private float m_mapScale;
        [SerializeField] private float m_iconScale;


        public EventAction<MapZoomEventActionArgs> OnMapZoom;

        public void SetupZoom(RectTransform receivedMap)
        {
            m_currentMap = receivedMap;
            m_currentY = DEFAULT_ZOOM;
            ApplyCurrentZoom();
        }

        public void SetZoomConstraints(float min, float max)
        {
            m_minZoom = min;
            m_maxZoom = max;
            m_currentY = Mathf.Clamp(m_currentY, m_minZoom, m_maxZoom);
            ApplyCurrentZoom();
        }

        public void Zoom()
        {
            if (m_currentMap == null || Mouse.current == null)
            {
                return;
            }

            var scrollWheel = Mouse.current.scroll.ReadValue();
            if (Mathf.Approximately(scrollWheel.y, 0f))
            {
                return;
            }

            var nextZoom = Mathf.Clamp(
                m_currentY + (m_mapScale * Mathf.Sign(scrollWheel.y)),
                m_minZoom,
                m_maxZoom);

            if (Mathf.Approximately(nextZoom, m_currentY))
            {
                return;
            }

            m_currentY = nextZoom;
            ApplyCurrentZoom();
            OnMapZoom?.Invoke(this, new MapZoomEventActionArgs(scrollWheel, m_iconScale));
        }

        private void ApplyCurrentZoom()
        {
            if (m_currentMap != null)
            {
                m_currentMap.localScale = Vector3.one * m_currentY;
            }
        }
    }
}
