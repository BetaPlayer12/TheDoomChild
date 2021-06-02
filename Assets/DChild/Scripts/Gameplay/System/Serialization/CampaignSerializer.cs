using DChild.Gameplay.Systems;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;
using System.Threading.Tasks;
using Doozy.Engine;
#if UNITY_EDITOR
using DChildDebug;
#endif
namespace DChild.Gameplay
{
    public class CampaignSlotUpdateEventArgs : IEventActionArgs
    {
        public CampaignSlot slot { get; private set; }

        public void Initialize(CampaignSlot slot)
        {
            this.slot = slot;
        }
    }

    public class CampaignSerializer : SerializedMonoBehaviour, IGameplaySystemModule
    {
#if UNITY_EDITOR
        [SerializeField, PropertyOrder(-1), FoldoutGroup("Debug")]
        private CampaignSlotData m_toLoad;
        [Button, PropertyOrder(-1), FoldoutGroup("Debug")]
        private void SetDataAsCurrentSlot()
        {
            m_slot = new CampaignSlot(m_toLoad.slot);
            using (Cache<CampaignSlotUpdateEventArgs> cacheEventArgs = Cache<CampaignSlotUpdateEventArgs>.Claim())
            {
                cacheEventArgs.Value.Initialize(m_slot);
                PostDeserialization?.Invoke(this, cacheEventArgs.Value);
                cacheEventArgs.Release();
            }
        }
#endif

        [OdinSerialize, HideReferenceObjectPicker]
        private CampaignSlot m_slot = new CampaignSlot();

        public event EventAction<CampaignSlotUpdateEventArgs> PreSerialization;
        public event EventAction<CampaignSlotUpdateEventArgs> PostDeserialization;
        public CampaignSlot slot => m_slot;

        public void SetSlot(CampaignSlot slot)
        {
            m_slot = slot;
            using (Cache<CampaignSlotUpdateEventArgs> cacheEventArgs = Cache<CampaignSlotUpdateEventArgs>.Claim())
            {
                cacheEventArgs.Value.Initialize(m_slot);
                PostDeserialization?.Invoke(this, cacheEventArgs.Value);
                cacheEventArgs.Release();
            }
        }

        [Button]
        public void Save()
        {
            GameEventMessage.SendEvent("Game Save Start");
            CallPreSerialization();
            SerializationHandle.SaveCampaignSlot(m_slot.id, m_slot);
        }

        public async Task<bool> SaveAsync()
        {
            GameEventMessage.SendEvent("Game Save Start");
            CallPreSerialization();
            await SerializationHandle.SaveCampaignSlotAsync(m_slot.id, m_slot);
            GameEventMessage.SendEvent("Game Save End");
            return true;
        }

        [Button]
        public void Load(bool bypassLoadingFromFile = false)
        {
            if (bypassLoadingFromFile == false)
            {
                SerializationHandle.LoadCampaignSlot(m_slot.id, ref m_slot);
            }
            CallPostDeserialization();
        }

        public void UpdateData()
        {
            CallPreSerialization();
        }

        public async Task<bool> LoadAsync()
        {
            GameEventMessage.SendEvent("Game Load Start");
            await SerializationHandle.LoadCampaignSlotAsync(m_slot.id,m_slot);
            CallPostDeserialization();
            GameEventMessage.SendEvent("Game Load End");
            return true;
        }

        private void CallPreSerialization()
        {
            using (Cache<CampaignSlotUpdateEventArgs> cacheEventArgs = Cache<CampaignSlotUpdateEventArgs>.Claim())
            {
                cacheEventArgs.Value.Initialize(m_slot);
                PreSerialization?.Invoke(this, cacheEventArgs.Value);
                cacheEventArgs.Release();
            }
        }
        private void CallPostDeserialization()
        {
            using (Cache<CampaignSlotUpdateEventArgs> cacheEventArgs = Cache<CampaignSlotUpdateEventArgs>.Claim())
            {
                cacheEventArgs.Value.Initialize(m_slot);
                PostDeserialization?.Invoke(this, cacheEventArgs.Value);
                cacheEventArgs.Release();
            }
        }
    }
}