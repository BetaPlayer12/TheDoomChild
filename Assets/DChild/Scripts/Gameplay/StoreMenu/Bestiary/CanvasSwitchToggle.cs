using UnityEngine;

namespace DChild.Menu.Bestiary
{
    public class CanvasSwitchToggle : MonoBehaviour, IToggleVisual
    {
        [SerializeField]
        private Canvas m_enableCanvas;
        [SerializeField]
        private Canvas m_disableCanvas;

        public void Toggle(bool value)
        {
            if (value)
            {
                m_enableCanvas.enabled = true;
                m_disableCanvas.enabled = false;
            }
            else
            {
                m_enableCanvas.enabled = false;
                m_disableCanvas.enabled = true;
            }
        }
    }
}