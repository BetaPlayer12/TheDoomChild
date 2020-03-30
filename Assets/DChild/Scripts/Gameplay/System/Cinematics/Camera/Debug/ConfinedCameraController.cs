using Holysoft;
using Sirenix.OdinInspector;
using UnityEngine;
using Cinemachine;


namespace DChildDebug.Gameplay.Camera
{
    [ExecuteInEditMode]
    public class ConfinedCameraController : MonoBehaviour
    {
        [SerializeField]
        private float m_speed = 1f;
        [SerializeField]
        [ReadOnly]
        private Vector3 m_direction;
        [SerializeField]
        [ReadOnly]
        private bool m_isControlled;
        [SerializeField]
        [ReadOnly]
        private CinemachineConfiner m_confiner;

        public bool isControlled => m_isControlled;

        public void SetControlValue(bool isControlled) => m_isControlled = isControlled;
        public void ToggleControl() => SetControlValue(!isControlled);

        private void Start()
        {
            m_confiner = GetComponentInChildren<CinemachineConfiner>();
        }

        private void OnGUI()
        {
            if (m_isControlled)
            {
                var currentEvent = Event.current;
                if (currentEvent != null && currentEvent.type == EventType.KeyDown)
                {
                    int vertical = 0;
                    int horizontal = 0;
                    switch (currentEvent.keyCode)
                    {
                        case KeyCode.UpArrow:
                            vertical = 1;
                            break;
                        case KeyCode.DownArrow:
                            vertical = -1;
                            break;
                        case KeyCode.LeftArrow:
                            horizontal = -1;
                            break;
                        case KeyCode.RightArrow:
                            horizontal = 1;
                            break;
                    }
                    m_direction = new Vector3(horizontal, vertical, 0f);
                    transform.position += m_direction * m_speed;
                    var hit = Physics2D.OverlapBox(transform.position, Vector2.one * 0.5f, 0, LayerMask.GetMask("CameraOnly"));
                    if (hit)
                    {
                        var boundingShape = hit.GetComponent<CompositeCollider2D>();
                        if (boundingShape != m_confiner.m_BoundingShape2D)
                        {
                            m_confiner.InvalidatePathCache();
                            m_confiner.m_BoundingShape2D = hit.GetComponent<CompositeCollider2D>();
                        }
                    }
                }
            }
        }
    }
}