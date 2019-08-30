using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Combat;
using Holysoft.Gameplay;
using DChild.Gameplay.Characters.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Skill
{
    public abstract class MagicSkill : MonoBehaviour, IComplexCharacterModule, IModifiableModule
    {
        [SerializeField, Min(0)]
        protected int m_baseMagicRequired;
        protected ICappedStat m_magic;

        protected int m_magicRequired;

        public virtual void ConnectTo(IPlayerModifer modifier)
        {
            m_magicRequired = Mathf.FloorToInt(m_baseMagicRequired * modifier.Get(PlayerModifier.Magic_Requirement));
            modifier.ModifierChange += OnModifierChange;
        }

        protected virtual void OnModifierChange(object sender, ModifierChangeEventArgs eventArgs)
        {
            if(eventArgs.modifier == PlayerModifier.Magic_Requirement)
            {
                m_magicRequired = Mathf.FloorToInt(m_baseMagicRequired * eventArgs.value);
            }
        }

        public virtual void Initialize(ComplexCharacterInfo info)
        {
            m_magic = info.magic;
        }

        public void DoSkill()
        {
            if (m_magic.currentValue < (m_magicRequired))
            {
                DenySkillUse();
            }
            else
            {
                UseSkill();
            }
        }

        protected virtual void UseSkill()
        {
            m_magic.ReduceCurrentValue(m_magicRequired);
        }

        protected virtual void DenySkillUse()
        {

        }


    }
}