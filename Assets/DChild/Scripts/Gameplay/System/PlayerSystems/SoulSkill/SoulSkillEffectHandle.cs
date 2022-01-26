using DChild.Gameplay.Characters.Players;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.SoulSkills
{
    public class SoulSkillEffectHandle : SerializedMonoBehaviour, ISoulSkillSlotList
    {
        [SerializeField]
        private IPlayer m_player;
        [SerializeField, MinValue(1)]
        private int m_maxCapacityLimit = 1;
        [ShowInInspector, HideInEditorMode]
        private SoulSkillSlotList m_slotList;

        public event EventAction<EventActionArgs> ChangeInContent;

        public int currentMaxCapacity => m_slotList.currentMaxCapacity;

        public int appliedSoulSkillCount => m_slotList.appliedSoulSkillCount;

        public ISoulSkill GetSoulSkill(int index) => m_slotList.GetSoulSkill(index);

        public void IncreaseMaxCapacity(int increaseValueBy)
        {
            m_slotList.SetMaxCapacity(m_slotList.currentMaxCapacity + increaseValueBy);
            LimitAppliedSkillsToCapacity();
            ChangeInContent?.Invoke(this, EventActionArgs.Empty);
        }

        public void DecreaseMaxCapacity(int decreaseValueBy)
        {
            m_slotList.SetMaxCapacity(m_slotList.currentMaxCapacity - decreaseValueBy);
            LimitAppliedSkillsToCapacity();
            ChangeInContent?.Invoke(this, EventActionArgs.Empty);
        }

        public void SetMaxCapacity(int maxCapacity)
        {
            m_slotList.SetMaxCapacity(maxCapacity);
            LimitAppliedSkillsToCapacity();
            ChangeInContent?.Invoke(this, EventActionArgs.Empty);
        }

        public void AddSkill(ISoulSkill soulSkill)
        {
            if (m_slotList.Add(soulSkill))
            {
                soulSkill.ApplyEffectTo(m_player);
                ChangeInContent?.Invoke(this, EventActionArgs.Empty);
            }
        }

        public void RemoveSkill(ISoulSkill soulSkill)
        {
            if (m_slotList.Remove(soulSkill))
            {
                soulSkill.RemoveEffectFrom(m_player);
                ChangeInContent?.Invoke(this, EventActionArgs.Empty);
            }
        }

        public void RemoveAllSkills()
        {
            var skillsRemoved = m_slotList.RemoveAllSkills();
            for (int i = 0; i < skillsRemoved.Length; i++)
            {
                skillsRemoved[i].RemoveEffectFrom(m_player);
            }
            ChangeInContent?.Invoke(this, EventActionArgs.Empty);
        }

        private void LimitAppliedSkillsToCapacity()
        {
            if (m_slotList.isAppliedSoulSkillsWithinCapacity == false)
            {
                var removedSkills = m_slotList.RemoveSoulSkillsAboveMaxCapacity();
                for (int i = 0; i < removedSkills.Length; i++)
                {
                    removedSkills[i].RemoveEffectFrom(m_player);
                }
            }
        }

        private void Awake()
        {
            m_slotList = new SoulSkillSlotList(m_maxCapacityLimit, 1);
        }
    }
}
