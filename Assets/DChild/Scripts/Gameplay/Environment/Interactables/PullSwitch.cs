/**************************************
 * 
 * A Generic Button that calls an event to 
 * those that are concerned only once.
 * After that the button will no longer function
 * 
 **************************************/

using DChild.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class PullSwitch : MonoBehaviour, ISerializableComponent
    {
        [System.Serializable]
        public struct SaveData : ISaveData
        {
            public SaveData(bool isOpen)
            {
                this.m_isPulled = isOpen;
            }

            [SerializeField]
            private bool m_isPulled;

            public bool isPulled => m_isPulled;

            ISaveData ISaveData.ProduceCopy() => new SaveData(m_isPulled);
        }

        [SerializeField]
        private Transform m_movableSwitch;
        [SerializeField, HorizontalGroup("StartPosition")]
        private Vector3 m_startingPosition;
        [SerializeField, HorizontalGroup("ActivatedPosition")]
        private Vector3 m_activatedPostion;

        [TabGroup("Main", "StartAs")]
        [SerializeField, TabGroup("Main/StartAs", "On")]
        private UnityEvent m_startAsOnState;
        [SerializeField, TabGroup("Main/StartAs", "Off")]
        private UnityEvent m_startAsOffState;

        [TabGroup("Main", "Transistion")]
        [SerializeField, TabGroup("Main/Transistion", "On")]
        private UnityEvent m_onState;

        private MovableObject m_movableObjectComponent;
        private bool m_isPulled;

        public void Load(ISaveData data)
        {
            m_isPulled = ((SaveData)data).isPulled;
            if (m_isPulled)
            {
                m_movableSwitch.position = m_activatedPostion;
                m_movableObjectComponent.SetMovable(false);
                m_startAsOnState?.Invoke();
            }
            else
            {
                m_movableSwitch.position = m_startingPosition;
                m_movableObjectComponent.SetMovable(true);
                m_startAsOffState?.Invoke();
            }
        }
        public void Initialize()
        {
            m_isPulled = false;
            m_movableSwitch.position = m_startingPosition;
            m_movableObjectComponent.SetMovable(true);
            m_startAsOffState?.Invoke();
        }
        public ISaveData Save() => new SaveData(m_isPulled);

        public void SetMovableState(bool isMovable)
        {
            if (m_isPulled) return;
            m_movableObjectComponent.SetMovable(isMovable);
        }

        private void Awake()
        {
            if (m_isPulled == false)
            {
                m_movableSwitch.position = m_startingPosition;
            }
            m_movableObjectComponent = GetComponentInChildren<MovableObject>();
            m_movableObjectComponent.SetMovable(true);
            enabled = false;

        }

        private void LateUpdate()
        {
            if (m_isPulled) return;

            if (m_movableSwitch.position == m_activatedPostion)
            {
                m_isPulled = true;
                m_movableObjectComponent.SetMovable(false);
                m_onState?.Invoke();
                enabled = false;
            }
        }

#if UNITY_EDITOR
        [Button("Use Current Position"), HorizontalGroup("StartPosition")]
        private void SetCurrentPositionAsStartPosition() => m_startingPosition = m_movableSwitch.position;

        [Button("Use Current Position"), HorizontalGroup("ActivatedPosition")]
        private void SetCurrentPositionAsActivatedPosition() => m_activatedPostion = m_movableSwitch.position;
#endif
    }
}