using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Gameplay
{
    public class UnlockRequirementCompleteEventArgs : IEventActionArgs
    {
        public void Initialize(UnlockRequirement instance, bool isComplete)
        {
            this.instance = instance;
            this.isComplete = isComplete;
        }

        public UnlockRequirement instance { get; private set; }
        public bool isComplete { get; private set; }
    }

    public interface IUnlockableRequirementSaveData : ISaveData
    {
        bool isComplete { get; }
    }

    public class UnlockRequirement : MonoBehaviour
    {
        [ShowInInspector, OnValueChanged("OnForceCompleteChange")]
        private bool m_isComplete;

        public bool isComplete { get => m_isComplete; }

        public event EventAction<UnlockRequirementCompleteEventArgs> CompletionChange;

        public void SetCompletion(bool isComplete)
        {
            m_isComplete = isComplete;
            using (Cache<UnlockRequirementCompleteEventArgs> cacheEvent = Cache<UnlockRequirementCompleteEventArgs>.Claim())
            {
                cacheEvent.Value.Initialize(this, m_isComplete);
                CompletionChange?.Invoke(this, cacheEvent);
                cacheEvent.Release();
            }
        }

#if UNITY_EDITOR
        private void OnForceCompleteChange()
        {
            using (Cache<UnlockRequirementCompleteEventArgs> cacheEvent = Cache<UnlockRequirementCompleteEventArgs>.Claim())
            {
                cacheEvent.Value.Initialize(this, m_isComplete);
                CompletionChange?.Invoke(this, cacheEvent);
                cacheEvent.Release();
            }
        }
#endif
    }
}