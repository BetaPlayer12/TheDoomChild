using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class DynamicFlowFallsBlocker : MonoBehaviour
    {
        [SerializeField]
        private Collider2D m_toBlock;
        [SerializeField, TabGroup("Block")]
        private UnityEvent m_onBlock;
        [SerializeField, TabGroup("Unblock")]
        private UnityEvent m_onUnblock;

        private bool m_hasBlocked;


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_hasBlocked == false)
            {
                if (m_toBlock == collision)
                {
                    m_onBlock?.Invoke();
                    m_hasBlocked = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (m_hasBlocked)
            {
                if (m_toBlock == collision)
                {
                    m_onUnblock?.Invoke();
                    m_hasBlocked = false;
                }
            }
        }
    }
}
