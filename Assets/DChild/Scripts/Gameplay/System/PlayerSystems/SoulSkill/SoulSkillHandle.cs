using DChild.Gameplay.Characters.Players.SoulSkills;
using DChild.Serialization;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public class SoulSkillHandle : SerializedMonoBehaviour
    {
        [SerializeField]
        private IPlayer m_source;
        [SerializeField, OnValueChanged("UpdateSkills"), BoxGroup("Attached Skills")]
        private SoulSkill m_armorSkill;
        [SerializeField, OnValueChanged("UpdateSkills"), BoxGroup("Attached Skills")]
        private SoulSkill m_supportSkill;
        [SerializeField, OnValueChanged("UpdateSkills",true), BoxGroup("Attached Skills")]
        private SoulSkill[] m_weaponSkills;

        public SoulSkillHandle()
        {
            m_weaponSkills = new SoulSkill[2];
            m_source = null;
#if UNITY_EDITOR
            m_prevWeaponSkills = new SoulSkill[2];
#endif
        }

        public EquippedSoulSkillData SaveData()
        {
            return new EquippedSoulSkillData(GetID(m_armorSkill), GetID(m_supportSkill),
                                            GetID(m_weaponSkills[0]), GetID(m_weaponSkills[1]));
        }

        private int GetID(SoulSkill soulSkill) => soulSkill?.id ?? -1;

        public void Initialize(IPlayer m_source)
        {
            m_weaponSkills = new SoulSkill[2];
            this.m_source = m_source;
#if UNITY_EDITOR
            m_prevWeaponSkills = new SoulSkill[2];
#endif
        }

        public void ClearAllSlots()
        {
            ClearSlot(ref m_armorSkill);
            ClearSlot(ref m_supportSkill);
            ClearSlot(ref m_weaponSkills[0]);
            ClearSlot(ref m_weaponSkills[1]);
        }
        
        public void AttachSkill(SoulSkill skill, int slotIndex = 0)
        {
            if (skill == null)
                return;

//            // We need to make this better
//            switch (skill.type)
//            {
//                case SoulSkillType.Armor:
//                    if (slotIndex > 0)
//                    {
//                        throw new System.Exception($"Armor Skills does not have index {slotIndex}");
//                    }
//                    else
//                    {
//                        AttachSkill(ref m_armorSkill, skill);
//#if UNITY_EDITOR
//                        m_prevArmorSkill = m_armorSkill;
//#endif
//                    }
//                    break;

//                case SoulSkillType.Support:
//                    if (slotIndex > 0)
//                    {
//                        throw new System.Exception($"Support Skills does not have index {slotIndex}");
//                    }
//                    else
//                    {
//                        AttachSkill(ref m_supportSkill, skill);
//#if UNITY_EDITOR
//                        m_prevSupportSkill = m_supportSkill;
//#endif
//                    }
//                    break;

//                case SoulSkillType.Weapon:
//                    if (slotIndex > m_weaponSkills.Length - 1)
//                    {
//                        throw new System.Exception($"Weapon Skills does not have index {slotIndex}");
//                    }
//                    else
//                    {
//                        AttachSkill(ref m_weaponSkills[slotIndex], skill);
//#if UNITY_EDITOR
//                        m_prevWeaponSkills[slotIndex] = m_weaponSkills[slotIndex];
//#endif
//                    }
//                    break;
//            }
        }

        public void DetachSkill(SoulSkill skill)
        {
//            switch (skill.type)
//            {
//                case SoulSkillType.Armor:
//                    if (m_armorSkill == skill)
//                    {
//                        ClearSlot(ref m_armorSkill);
//#if UNITY_EDITOR
//                        m_prevArmorSkill = null;
//#endif
//                    }
//                    break;

//                case SoulSkillType.Support:
//                    if (m_supportSkill == skill)
//                    {
//                        ClearSlot(ref m_supportSkill);
//#if UNITY_EDITOR
//                        m_prevSupportSkill = null;
//#endif
//                    }
//                    break;

//                case SoulSkillType.Weapon:
//                    for (int i = 0; i < m_weaponSkills.Length; i++)
//                    {
//                        if (m_weaponSkills[i] == skill)
//                        {
//                            ClearSlot(ref m_weaponSkills[i]);
//#if UNITY_EDITOR
//                            m_prevWeaponSkills[i] = null;
//#endif
//                            break;
//                        }
//                    }
//                    break;
//            }
        }

        private void AttachSkill(ref SoulSkill slot, SoulSkill skill)
        {
            if (m_source != null)
            {
                slot?.DetachFrom(m_source);
                skill?.AttachTo(m_source);
            }
            slot = skill;
        }

        private void ClearSlot(ref SoulSkill slot)
        {
            slot?.DetachFrom(m_source);
            slot = null;
        }

#if UNITY_EDITOR
        private SoulSkill m_prevArmorSkill;
        private SoulSkill m_prevSupportSkill;
        private SoulSkill[] m_prevWeaponSkills;

        private void UpdateSkills()
        {
            UpdateReference(ref m_armorSkill, ref m_prevArmorSkill);
            UpdateReference(ref m_supportSkill, ref m_prevSupportSkill);
            UpdateReference(ref m_weaponSkills[0], ref m_prevWeaponSkills[0]);
            UpdateReference(ref m_weaponSkills[1], ref m_prevWeaponSkills[1]);
        }

        private void UpdateReference(ref SoulSkill current, ref SoulSkill prev)
        {
            if (current != prev)
            {
                prev?.DetachFrom(m_source);
                prev = current;
                current?.AttachTo(m_source);
            }
        }
#endif
    }
}
