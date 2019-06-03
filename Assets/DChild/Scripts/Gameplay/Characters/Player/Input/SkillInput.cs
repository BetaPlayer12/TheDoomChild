using UnityEngine;

namespace DChild.Inputs
{
    [System.Serializable]
    public struct SkillInput
    {
        private const string INPUT_DASH = "Dash";
        private const string INPUT_GLIDE = "Glide";

        private bool m_isDashPressed;
        private bool m_isGlideHeld;

        public bool isDashPressed => m_isDashPressed;
        public bool isGlideHeld => m_isGlideHeld;

        public void Disable()
        {
            m_isDashPressed = false;
            m_isGlideHeld = false;
        }

        public void Update()
        {
            m_isDashPressed = Input.GetButtonDown(INPUT_DASH);
            m_isGlideHeld = Input.GetButton(INPUT_GLIDE);
        }
    }
}
