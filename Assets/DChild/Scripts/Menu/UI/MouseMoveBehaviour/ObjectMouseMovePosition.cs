using UnityEngine;

namespace DChild.Menu
{
    public class ObjectMouseMovePosition : MouseMovePositionBehaviour
    {
        private Vector3 m_position;
        protected override Vector2 value
        {
            get => transform.localPosition;
            set
            {
                m_position.x = value.x;
                m_position.y = value.y;
                transform.localPosition = m_position;
            }
        }

        protected override void Awake()
        {
            m_position = transform.localPosition;
            base.Awake();
        }
    }
}