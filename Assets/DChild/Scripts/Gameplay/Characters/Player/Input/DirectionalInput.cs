using UnityEngine;

namespace DChild.Inputs
{

    [System.Serializable]
    public struct DirectionalInput : IInput
    {
        private const string INPUT_HORIZONTAL = "Horizontal";
        private const string INPUT_VERTICAL = "Vertical";

        private float m_horizontalInput;
        private bool m_isHorizontalHeld;
        private bool m_isRightHeld;
        private bool m_isLeftHeld;
        private bool m_isRightPressed;
        private bool m_isLeftPressed;

        private float m_verticalInput;
        private bool m_isVerticalHeld;
        private bool m_isUpHeld;
        private bool m_isDownHeld;
        private bool m_isUpPressed;
        private bool m_isDownPressed;

        public float horizontalInput => m_horizontalInput;
        public bool isHorizontalHeld => m_isHorizontalHeld;
        public bool isRightHeld => m_isRightHeld;
        public bool isLeftHeld => m_isLeftHeld;
        public bool isRightPressed => m_isRightPressed;
        public bool isLeftPressed => m_isLeftPressed;

        public float verticalInput => m_verticalInput;
        public bool isVerticalHeld => m_isVerticalHeld;
        public bool isUpHeld => m_isUpHeld;
        public bool isDownHeld => m_isDownHeld;
        public bool isUpPressed => m_isUpPressed;
        public bool isDownPressed => m_isDownPressed;

        public void Disable()
        {
            m_horizontalInput = 0;
            m_isHorizontalHeld = false;
            m_isRightHeld = false;
            m_isLeftHeld = false;
            m_isRightPressed = false;
            m_isLeftPressed = false;

            m_verticalInput = 0;
            m_isVerticalHeld = false;
            m_isUpHeld = false;
            m_isDownHeld = false;
            m_isUpPressed = false;
            m_isDownPressed = false;
        }

        public void Reset()
        {
            throw new System.NotImplementedException();
        }

        public void Update()
        {
            HandleHorizontalInput();
            HandleVerticalInput();
        }

        private void HandleHorizontalInput()
        {
            m_horizontalInput = Input.GetAxisRaw(INPUT_HORIZONTAL);
            m_isRightHeld = m_horizontalInput > 0;
            m_isLeftHeld = m_horizontalInput < 0;
            m_isHorizontalHeld = m_isRightHeld || m_isLeftHeld;

            if (Input.GetButtonDown(INPUT_HORIZONTAL))
            {
                m_isRightPressed = m_isRightHeld;
                m_isLeftPressed = m_isLeftHeld;
            }
            else
            {
                m_isRightPressed = false;
                m_isLeftPressed = false;
            }
        }

        private void HandleVerticalInput()
        {

            m_verticalInput = Input.GetAxisRaw(INPUT_VERTICAL);
            m_isUpHeld = m_verticalInput > 0;
            m_isDownHeld = m_verticalInput < 0;
            m_isVerticalHeld = m_isUpHeld || m_isDownHeld;

            if (Input.GetButtonDown(INPUT_VERTICAL))
            {
                m_isUpPressed = m_isUpHeld;
                m_isDownPressed = m_isDownHeld;
            }
            else
            {
                m_isUpPressed = false;
                m_isDownPressed = false;
            }
        }
    }

}