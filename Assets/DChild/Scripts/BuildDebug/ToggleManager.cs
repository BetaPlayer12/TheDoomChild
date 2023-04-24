using Doozy.Runtime.UIManager.Components;
using UnityEngine;

namespace DChildDebug.Window
{
    public class ToggleManager : MonoBehaviour
    {
        private UIToggle[] m_toggles;

        public void ToggleOnAll()
        {
            for (int i = 0; i < m_toggles.Length; i++)
            {
                m_toggles[i].SetIsOn(true);
            }
        }
        public void ToggleOffAll()
        {
            for (int i = 0; i < m_toggles.Length; i++)
            {
                m_toggles[i].SetIsOn(false);
            }
        }

        private void Awake()
        {
            m_toggles = GetComponentsInChildren<UIToggle>();
        }
    }

}