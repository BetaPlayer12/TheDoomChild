using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay
{
    public class UnlockableEvent : MonoBehaviour
    {
        [System.Serializable]
        private class Requirement
        {
            [SerializeField,SceneObjectsOnly]
            private UnlockRequirement m_requirement;

            [FoldoutGroup("Reactions")]

            [SerializeField,HorizontalGroup("Reactions/Split")]
            private UnityEvent m_onComplete;
            [SerializeField, HorizontalGroup("Reactions/Split")]
            private UnityEvent m_onIncomplete;

            public event EventAction<UnlockRequirementCompleteEventArgs> CompletionChange
            {
                add
                {
                    m_requirement.CompletionChange += value;
                }

                remove
                {
                    m_requirement.CompletionChange -= value;
                }
            }

            public bool isComplete => m_requirement.isComplete;

            public bool IsInstance(UnlockRequirement compareTo) => m_requirement == compareTo;

            public void UpdateReaction()
            {
                if (m_requirement.isComplete)
                {
                    m_onComplete?.Invoke();
                }
                else
                {
                    m_onIncomplete?.Invoke();
                }
            }
        }

        [ShowInInspector, OnValueChanged("OnStateChange")]
        private bool m_isUnlocked;
        [SerializeField, TabGroup("Requirements")]
        private Requirement[] m_requirements;
        [SerializeField, TabGroup("Unlocked")]
        private UnityEvent m_onUnlocked;
        [SerializeField, TabGroup("Locked")]
        private UnityEvent m_onLocked;

        private int m_requirementCompleteCount;

        public void SetLock(bool isLocked)
        {
            m_isUnlocked = !isLocked;
            if (m_isUnlocked)
            {
                m_onUnlocked?.Invoke();
            }
            else
            {
                m_onLocked?.Invoke();
            }
        }

        public void CheckRequirementStates()
        {
            m_requirementCompleteCount = 0;
            for (int i = 0; i < m_requirements.Length; i++)
            {
                if (m_requirements[i].isComplete)
                {
                    m_requirementCompleteCount++;
                }
            }
            UpdateState();
        }

        private void UpdateState()
        {
            SetLock(m_requirementCompleteCount < m_requirements.Length);
        }

        private void OnRequirementComplete(object sender, UnlockRequirementCompleteEventArgs eventArgs)
        {
            m_requirementCompleteCount += eventArgs.isComplete ? 1 : -1;
            for (int i = 0; i < m_requirements.Length; i++)
            {
                if (m_requirements[i].IsInstance(eventArgs.instance))
                {
                    m_requirements[i].UpdateReaction();
                    break;
                }
            }
            UpdateState();
        }

        private void Start()
        {
            m_requirementCompleteCount = 0;
            for (int i = 0; i < m_requirements.Length; i++)
            {
                m_requirements[i].CompletionChange += OnRequirementComplete;
                if (m_requirements[i].isComplete)
                {
                    m_requirementCompleteCount++;
                }
            }
            UpdateState();
        }

#if UNITY_EDITOR
        private void OnStateChange()
        {
            if (m_isUnlocked)
            {
                m_onUnlocked?.Invoke();
            }
            else
            {
                m_onLocked?.Invoke();
            }
        }
#endif
    }
}