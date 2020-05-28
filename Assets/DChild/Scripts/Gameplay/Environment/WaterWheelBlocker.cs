using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class WaterWheelBlocker : MonoBehaviour
    {
        [SerializeField]
        private Collider2D m_toBlock;
        [SerializeField, TabGroup("Block")]
        private UnityEvent m_onBlock;
        [SerializeField, TabGroup("Unblock")]
        private UnityEvent m_onUnblock;


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_toBlock == collision)
            {
                m_onBlock?.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (m_toBlock == collision)
            {
                m_onUnblock?.Invoke();
            }
        }
    }
}
