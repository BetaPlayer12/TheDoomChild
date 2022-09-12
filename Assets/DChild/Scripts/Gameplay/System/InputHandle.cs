using UnityEngine;
using UnityEngine.InputSystem;

namespace DChild.Gameplay.Systems
{
    public abstract class InputHandle : MonoBehaviour
    {
        protected InputActionMap m_actionMap;
        private bool m_isSubscribed;

        public virtual void SetActionMap(InputActionMap actionMap)
        {
            if (m_actionMap == null)
            {
                m_actionMap = actionMap;
                if (enabled && m_isSubscribed == false)
                {
                    SubscribeToActionMap(m_actionMap);
                    m_isSubscribed = true;
                }
            }
            else if(m_actionMap != actionMap)
            {
                UnsubscribeToActionMap(m_actionMap);
                m_actionMap = actionMap;
                if (enabled)
                {
                    SubscribeToActionMap(m_actionMap);
                }
            }
        }

        protected abstract void SubscribeToActionMap(InputActionMap actionMap);
        protected abstract void UnsubscribeToActionMap(InputActionMap actionMap);

        private void OnEnable()
        {
            if(m_actionMap != null && m_isSubscribed == false)
            {
                SubscribeToActionMap(m_actionMap);
                m_isSubscribed = true;
            }
        }

        private void OnDisable()
        {
            if (m_actionMap != null && m_isSubscribed)
            {
                UnsubscribeToActionMap(m_actionMap);
                m_isSubscribed = false;
            }
        }
    }
}