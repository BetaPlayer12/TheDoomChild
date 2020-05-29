using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class AbyssmalGateSlot : MonoBehaviour
    {
        [SerializeField, TabGroup("Activated")]
        private UnityEvent m_onActivated;
        [SerializeField, TabGroup("Transistion")]
        private UnityEvent m_transistion;
        [SerializeField, TabGroup("Deactivated")]
        private UnityEvent m_onDeactivated;

        private bool m_useTransistion;
        private bool m_isActive;

        public void SetActive(bool isActive)
        {
            if (isActive)
            {
                if (m_useTransistion)
                {
                    m_onActivated?.Invoke();
                }
                else
                {
                    m_transistion?.Invoke();
                }
            }
            else
            {
                m_onDeactivated?.Invoke();
            }
            m_isActive = isActive;
        }

        private void Start()
        {
            m_useTransistion = true;
        }
    }

}