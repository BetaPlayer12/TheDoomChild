using Sirenix.OdinInspector;
using UnityEngine;

namespace DChildDebug.Window
{
    public class ToggleCombatUI : MonoBehaviour, IToggleDebugBehaviour
    {
        [SerializeField]
        private Canvas m_combatUI;

        public bool value => m_combatUI.gameObject.activeInHierarchy;

        [Button]
        public void ToggleOn()
        {
            m_combatUI.gameObject.SetActive(true);
        }

        [Button]
        public void ToggleOff()
        {
            m_combatUI.gameObject.SetActive(false);
        }
    }
}
