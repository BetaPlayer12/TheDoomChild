﻿using DChild.Gameplay.Systems;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;
using System.Threading.Tasks;
using DChild.Temp;
using System;
using PixelCrushers.DialogueSystem;
using PixelCrushers;
using Doozy.Runtime.Signals;
#if UNITY_EDITOR
using DChildDebug;
#endif
namespace DChild.Gameplay
{
    [Flags]
    public enum SerializationScope
    {
        Player = 1 << 0,
        Zone = 1 << 1,
        Quest = 1 << 2,
        Menu = 1 << 3,
        Gameplay = Player | Zone | Quest,
    }

    public class CampaignSlotUpdateEventArgs : IEventActionArgs
    {
        private SerializationScope m_scope;
        public CampaignSlot slot { get; private set; }

        public void Initialize(CampaignSlot slot, SerializationScope scope)
        {
            this.slot = slot;
            m_scope = scope;
        }

        public bool IsPartOfTheUpdate(SerializationScope scope)
        {
            return m_scope.HasFlag(scope);
        }
    }

    public class CampaignSerializer : SerializedMonoBehaviour, IGameplaySystemModule
    {
        [SerializeField]
        private SignalSender m_serializationSignal;
#if UNITY_EDITOR
        [SerializeField, PropertyOrder(-1), FoldoutGroup("Debug")]
        private CampaignSlotData m_toLoad;
        [Button, PropertyOrder(-1), FoldoutGroup("Debug")]
        private void SetDataAsCurrentSlot(SerializationScope scope)
        {
            m_slot = new CampaignSlot(m_toLoad.slot);
            using (Cache<CampaignSlotUpdateEventArgs> cacheEventArgs = Cache<CampaignSlotUpdateEventArgs>.Claim())
            {
                cacheEventArgs.Value.Initialize(m_slot, scope);
                PostDeserialization?.Invoke(this, cacheEventArgs.Value);
                cacheEventArgs.Release();
            }
        }
#endif

        [OdinSerialize, HideReferenceObjectPicker]
        private CampaignSlot m_slot = new CampaignSlot();

        private bool m_willLoadDialogueSerializedData;
        private string m_loadDialogueSerializedData;

        public event EventAction<CampaignSlotUpdateEventArgs> PreSerialization;
        public event EventAction<CampaignSlotUpdateEventArgs> PostDeserialization;
        public CampaignSlot slot => m_slot;

        public void SetSlot(CampaignSlot slot)
        {
            m_slot = slot;
            using (Cache<CampaignSlotUpdateEventArgs> cacheEventArgs = Cache<CampaignSlotUpdateEventArgs>.Claim())
            {
                cacheEventArgs.Value.Initialize(m_slot, SerializationScope.Gameplay);
                PostDeserialization?.Invoke(this, cacheEventArgs.Value);
                cacheEventArgs.Release();
            }
        }

        [Button]
        public void Save(SerializationScope scope)
        {
            m_serializationSignal.SendSignal();
            m_slot.SetAsNewGame(false);
            CallPreSerialization(scope);
            if (m_slot.allowWriteToDisk)
            {
                SerializationHandle.SaveCampaignSlot(m_slot.id, m_slot);
            }
        }

        public async Task<bool> SaveAsync(SerializationScope scope)
        {
            CallPreSerialization(scope);
            await SerializationHandle.SaveCampaignSlotAsync(m_slot.id, m_slot);
            return true;
        }

        [Button]
        public void Load(SerializationScope scope, bool bypassLoadingFromFile = false)
        {
            if (bypassLoadingFromFile == false)
            {
                SerializationHandle.LoadCampaignSlot(m_slot.id, ref m_slot);
            }
            CallPostDeserialization(scope);
        }

        public void UpdateData(SerializationScope scope)
        {
            CallPreSerialization(scope);
        }

        public async Task<bool> LoadAsync(SerializationScope scope)
        {
            await SerializationHandle.LoadCampaignSlotAsync(m_slot.id,m_slot);
            CallPostDeserialization(scope);
            return true;
        }

        private void CallPreSerialization(SerializationScope scope)
        {
            using (Cache<CampaignSlotUpdateEventArgs> cacheEventArgs = Cache<CampaignSlotUpdateEventArgs>.Claim())
            {
                cacheEventArgs.Value.Initialize(m_slot, scope);
                PreSerialization?.Invoke(this, cacheEventArgs.Value);
                cacheEventArgs.Release();
            }
        }
        private void CallPostDeserialization(SerializationScope scope)
        {
            using (Cache<CampaignSlotUpdateEventArgs> cacheEventArgs = Cache<CampaignSlotUpdateEventArgs>.Claim())
            {
                cacheEventArgs.Value.Initialize(m_slot, scope);
                PostDeserialization?.Invoke(this, cacheEventArgs.Value);
                cacheEventArgs.Release();
            }
        }
        private void OnDialogueDatabaseAdded()
        {
            PersistentDataManager.ApplySaveData(m_slot.dialogueSaveData);
        }

        private void Start()
        {
            PersistentDataManager.ApplySaveData(m_slot.dialogueSaveData);
            ExtraDatabases.addedDatabases += OnDialogueDatabaseAdded;
        }

        public void UpdateDialogueSaveData()
        {
            m_slot.UpdateDialogueSaveData();
        }
    }
}