using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{

    public class MultiLockDoor : MonoBehaviour, ISerializableComponent
    {

    

        [System.Serializable]
        public struct SaveData : ISaveData
        {
            [SerializeField]
            private int m_currentUnlockCount;
            [SerializeField]
            private bool m_isOpen;

            public SaveData(int currentUnlockCount, bool isOpen)
            {
                m_currentUnlockCount = currentUnlockCount;
                m_isOpen = isOpen;
            }

            public int currentUnlockCount => m_currentUnlockCount;
            public bool isOpen => m_isOpen;

            ISaveData ISaveData.ProduceCopy() => new SaveData(m_currentUnlockCount, m_isOpen);
        }

        public struct UnlockEvent : IEventActionArgs
        {
            public UnlockEvent(int currentUnlockCount, bool isOpen)
            {
                this.currentUnlockCount = currentUnlockCount;
                this.isOpen = isOpen;
            }

            public int currentUnlockCount { get; }
            public bool isOpen { get; }
        }

        [SerializeField]
        private int m_maxLockCount;

        [SerializeField]
        private int m_currentUnlockCount;
        private Collider2D m_collider;

        public event EventAction<UnlockEvent> OnLockChange;

        public int currentUnlockCount => m_currentUnlockCount;
        public bool isOpen => m_currentUnlockCount >= m_maxLockCount;

        public void Initialize()
        {
            m_collider = GetComponent<Collider2D>();
        }

        public void Load(ISaveData data)
        {
            SetUnlockCount(((SaveData)data).currentUnlockCount);
        }

        public ISaveData Save() => new SaveData(m_currentUnlockCount, isOpen);

        public void ResetLock()
        {
            SetUnlockCount(0);
        }

        public void AdvanceUnlock()
        {
            SetUnlockCount(m_currentUnlockCount + 1);
        }

        private void SetUnlockCount(int unlockCount)
        {
            m_currentUnlockCount = unlockCount;
            m_collider.enabled = !isOpen;
            OnLockChange?.Invoke(this, new UnlockEvent(m_currentUnlockCount, isOpen));
        }

        private void Awake()
        {
            Initialize();
            Debug.Log("Door Awake" + "Door Unlocked Count: " + m_currentUnlockCount);
        }
    }

}