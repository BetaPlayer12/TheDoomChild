using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay
{
    public class UnlockRequirementCompleteEventArgs : IEventActionArgs
    {
        public UnlockRequirementCompleteEventArgs(UnlockRequirement instance)
        {
            this.instance = instance;
        }

        public UnlockRequirement instance { get; private set; }
        public bool isComplete { get; private set; }
        public void Set(bool isComplete) => this.isComplete = isComplete;
    }

    public class UnlockRequirement : MonoBehaviour
    {
        [ShowInInspector, OnValueChanged("OnForceCompleteChange")]
        private bool m_isComplete;

        public bool isComplete { get => m_isComplete; }

        public event EventAction<UnlockRequirementCompleteEventArgs> CompletionChange;
        private UnlockRequirementCompleteEventArgs m_eventArgs;

        public void SetCompletion(bool isComplete)
        {
            m_isComplete = isComplete;
            m_eventArgs.Set(m_isComplete);
            CompletionChange?.Invoke(this, m_eventArgs);
        }

        private void Awake()
        {
            m_eventArgs = new UnlockRequirementCompleteEventArgs(this);
        }

#if UNITY_EDITOR
        private void OnForceCompleteChange()
        {
            m_eventArgs.Set(m_isComplete);
            CompletionChange?.Invoke(this, m_eventArgs);
        }
#endif
    }
}