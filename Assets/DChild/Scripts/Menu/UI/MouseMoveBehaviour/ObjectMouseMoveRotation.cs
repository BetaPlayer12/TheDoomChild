using UnityEngine;

namespace DChild.Menu
{
    public class ObjectMouseMoveRotation : MouseMoveRotationBehaviour
    {
        private Vector3 m_rotation;
        protected override Vector2 value
        {
            get => m_rotation;
            set
            {
                m_rotation.x = value.x;
                m_rotation.y = value.y;
                transform.localRotation = Quaternion.Euler(m_rotation);
            }
        }

        protected override void Awake()
        {
            m_rotation = transform.localRotation.eulerAngles;
            base.Awake();
        }
    }
}