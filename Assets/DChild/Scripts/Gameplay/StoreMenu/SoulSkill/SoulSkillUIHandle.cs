using DChild.Gameplay.Characters.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Menu.SoulSkills
{
    public class SoulSkillUIHandle : MonoBehaviour
    {
        [SerializeField]
        private SoulSkillHandle m_soulSkillHandle;
        [SerializeField]
        private SoulSkillSlot[] m_slots;

        private SoulSkillUI m_selectedUI;

        public void Select(SoulSkillUI soulSkillUI)
        {
            m_selectedUI = soulSkillUI;
        }

        private void OnSlotInsertion(object sender, SoulSkillSlot.SoulSkillSlotEventArgs eventArgs)
        {
            //if (eventArgs.instance.slotType == m_selectedUI.data.type)
            //{
            //    eventArgs.instance.SetContainedUI(m_selectedUI);
            //    m_soulSkillHandle.AttachSkill(m_selectedUI.data, eventArgs.instance.index);
            //    m_selectedUI = null;
            //}
            //else
            //{
            //    m_selectedUI = null;
            //}
        }

        private void OnSlotRemoval(object sender, SoulSkillSlot.SoulSkillSlotEventArgs eventArgs)
        {
            m_soulSkillHandle.DetachSkill(eventArgs.instance.containedUI.data);
            m_selectedUI = null;
        }

        private void Awake()
        {
            for (int i = 0; i < m_slots.Length; i++)
            {
                m_slots[i].AttemptToBeInserted += OnSlotInsertion;
                m_slots[i].AttemptSkillRemoval += OnSlotRemoval;
            }
        }
    }

}