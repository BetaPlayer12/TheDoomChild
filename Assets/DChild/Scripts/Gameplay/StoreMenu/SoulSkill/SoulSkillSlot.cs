using System;
using DChild.Gameplay.Characters.Players;
using Holysoft.Event;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DChild.Menu.SoulSkills
{
    public class SoulSkillSlot : MonoBehaviour
    {
        public class SoulSkillSlotEventArgs : IEventActionArgs
        {
            public SoulSkillSlot instance { get; private set; }

            public void Initialize(SoulSkillSlot instance) => this.instance = instance;
        }

        [SerializeField]
        private int m_index;

        private SoulSkillUI m_containedUI;

        public event EventAction<SoulSkillSlotEventArgs> AttemptToBeInserted;
        public event EventAction<SoulSkillSlotEventArgs> AttemptSkillRemoval;

        public SoulSkillUI containedUI => m_containedUI;
        public int index { get => m_index; }

        public void SetContainedUI(SoulSkillUI soulSkillUI)
        {
            if (soulSkillUI == null)
            {
                m_containedUI.ReplacementAttempt -= OnReplacementAttempt;
                m_containedUI.ReturnToOriginalPosition();
                m_containedUI = null;
            }
            else
            {
                if (m_containedUI != null)
                {
                    m_containedUI.ReplacementAttempt -= OnReplacementAttempt;
                    m_containedUI.ReturnToOriginalPosition();
                }
                m_containedUI = soulSkillUI;
                m_containedUI.ReplacementAttempt += OnReplacementAttempt;
                m_containedUI.transform.SetParent(transform);
            }
        }

        public void SendInsertionAttempEvent()
        {
            using (Cache<SoulSkillSlotEventArgs> cacheEventArgs = Cache<SoulSkillSlotEventArgs>.Claim())
            {
                cacheEventArgs.Value.Initialize(this);
                AttemptToBeInserted?.Invoke(this, cacheEventArgs.Value);
                cacheEventArgs.Release();
            }
        }

        public void SendSkillRemovalAttempEvent()
        {
            using (Cache<SoulSkillSlotEventArgs> cacheEventArgs = Cache<SoulSkillSlotEventArgs>.Claim())
            {
                cacheEventArgs.Value.Initialize(this);
                AttemptSkillRemoval?.Invoke(this, cacheEventArgs.Value);
                cacheEventArgs.Release();
            }
        }

        private void OnReplacementAttempt(object sender, EventActionArgs eventArgs)
        {
            SendInsertionAttempEvent();
        }
    }
}