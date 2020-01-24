using DChild.Gameplay;
using DChild.Gameplay.Environment;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    [System.Serializable]
    public class AnimatedSwitchHandle : ISwitchHandle
    {
        [SerializeField, MinValue(0.1)]
        private float m_transitionDelay;
        [SerializeField, HideReferenceObjectPicker]
        private UnityEvent m_onEntrance = new UnityEvent();
        [SerializeField, HideReferenceObjectPicker]
        private UnityEvent m_onExit = new UnityEvent();

        public float transitionDelay => m_transitionDelay;

        public void DoSceneTransition(Character character, TransitionType type)
        {
            if (type == TransitionType.Enter)
            {
                m_onEntrance?.Invoke();
            }
            else if (type == TransitionType.Exit)
            {
                m_onExit?.Invoke();
            }
        }
    } 
}
