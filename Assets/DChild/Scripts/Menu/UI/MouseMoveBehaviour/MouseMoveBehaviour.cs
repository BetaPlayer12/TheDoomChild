using Holysoft.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DChild.Menu
{
    public abstract class MouseMoveBehaviour : UIBehaviour
    {
        [SerializeField]
        private Vector2 m_movementThreshold;
        [SerializeField]
        private bool m_inverseX;
        [SerializeField]
        private bool m_inverseY;
        [SerializeField]
        [MinValue(0)]
        private float m_maxMouseMoveThreshold;
        [SerializeField]
        [MinValue(0)]
        private float m_speed;

        private Vector2 m_initialValue;
        private Vector2 m_distanceFromInitialValue;
        private Vector2 m_prevMousePosition;
        protected Vector2 m_mouseMovement;
        private Vector2 m_movement;

        protected abstract Vector2 value { get; set; }
        private Vector2 mousePosition => Mouse.current.position.ReadValue();

        protected virtual void Awake()
        {
            m_initialValue = value;
            m_distanceFromInitialValue = Vector2.zero;
            m_prevMousePosition = mousePosition;
        }

        private void OnEnable()
        {
            value = m_initialValue;
            m_distanceFromInitialValue = Vector2.zero;
            m_prevMousePosition = mousePosition;
        }

        private void Update()
        {
            var currentMousePosition = mousePosition;
            if (m_prevMousePosition != currentMousePosition)
            {
                CalculateMouseMovement(currentMousePosition);
                TranslateMouseToUIMovement();
                value += (m_movement * Time.deltaTime);
                m_distanceFromInitialValue = value - m_initialValue;
                ClampToThreshold();

            }
            m_prevMousePosition = currentMousePosition;
        }

        protected abstract float TranslateMovementX();
        protected abstract float TranslateMovementY();

        private void TranslateMouseToUIMovement()
        {
            if (Mathf.Abs(m_distanceFromInitialValue.x) <= m_movementThreshold.x)
            {
                m_movement.x = m_maxMouseMoveThreshold * TranslateMovementX() * (m_inverseX ? -1 : 1) * m_speed;
            }
            else
            {
                m_movement.x = 0;
            }
            if (Mathf.Abs(m_distanceFromInitialValue.y) <= m_movementThreshold.y)
            {
                m_movement.y = m_maxMouseMoveThreshold * TranslateMovementY() * (m_inverseY ? -1 : 1) * m_speed;
            }
            else
            {
                m_movement.y = 0;
            }
        }

        private void CalculateMouseMovement(Vector2 currentMousePosition)
        {
            var displacement = currentMousePosition - m_prevMousePosition;
            m_mouseMovement.x = Mathf.Clamp(displacement.x / m_maxMouseMoveThreshold, -1, 1);
            m_mouseMovement.y = Mathf.Clamp(displacement.y / m_maxMouseMoveThreshold, -1, 1);
        }

        private void ClampToThreshold()
        {
            var currentPosition = value;
            if (Mathf.Abs(m_distanceFromInitialValue.x) > m_movementThreshold.x)
            {
                var direction = Mathf.Sign(m_distanceFromInitialValue.x);
                currentPosition.x = m_initialValue.x + (m_movementThreshold.x * direction);
                m_distanceFromInitialValue.x = m_movementThreshold.x * direction;
            }
            if (Mathf.Abs(m_distanceFromInitialValue.y) > m_movementThreshold.y)
            {
                var direction = Mathf.Sign(m_distanceFromInitialValue.y);
                currentPosition.y = m_initialValue.y + (m_movementThreshold.y * direction);
                m_distanceFromInitialValue.y = m_movementThreshold.y * direction;
            }
            value = currentPosition;
        }
    }
}