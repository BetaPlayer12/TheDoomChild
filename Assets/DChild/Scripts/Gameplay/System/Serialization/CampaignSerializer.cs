using DChild.Gameplay.Systems;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay
{

    public class CampaignSlotUpdateEventArgs : IEventActionArgs
    {
        public CampaignSlot slot { get; private set; }

        public void Set(CampaignSlot slot)
        {
            this.slot = slot;
        }
    }

    public class CampaignSerializer : MonoBehaviour, IGameplaySystemModule, IGameplayInitializable
    {
        [SerializeField]
        private CampaignSlot m_slot;

        private CampaignSlotUpdateEventArgs m_eventArgs;

        public event EventAction<CampaignSlotUpdateEventArgs> PreSerialization;
        public event EventAction<CampaignSlotUpdateEventArgs> PostDeserialization;
        public CampaignSlot slot => m_slot;

        public void SetSlot(CampaignSlot slot)
        {
            m_slot = slot;
            m_eventArgs.Set(m_slot);
            PostDeserialization?.Invoke(this, m_eventArgs);
        }

        [Button]
        public void Save()
        {
            PreSerialization?.Invoke(this, m_eventArgs);
            SerializationHandle.Save(m_slot.id, m_slot);
        }

        [Button]
        public void Load()
        {
            SerializationHandle.Load(m_slot.id, ref m_slot);
            PostDeserialization?.Invoke(this, m_eventArgs);
        }

        public void Initialize()
        {
            m_eventArgs = new CampaignSlotUpdateEventArgs();
            m_eventArgs.Set(m_slot);
        }
    }
}