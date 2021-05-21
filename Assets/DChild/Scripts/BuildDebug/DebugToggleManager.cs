using UnityEngine;

namespace DChildDebug.Window
{
    public class DebugToggleManager : MonoBehaviour
    {
        private DebugToggle[] m_toggles;

        public void UpdateAllToggleState()
        {
            for (int i = 0; i < m_toggles.Length; i++)
            {
                m_toggles[i].UpdateToggleHighlight();
            }
        }

        private void Start()
        {
            m_toggles = GetComponentsInChildren<DebugToggle>();
        }
    }

}