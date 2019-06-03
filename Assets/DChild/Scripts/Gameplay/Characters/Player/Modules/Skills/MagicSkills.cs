using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Skill
{
    public abstract class MagicSkill : MonoBehaviour, IPlayerExternalModule
    {
        protected class Module
        {
            private IPlayerModules m_source;

            public Module(IPlayerModules m_source)
            {
                this.m_source = m_source;
            }

            public IMagicModifier modifier => m_source.modifiers;
            public ICappedStat magic => m_source.magic;
        }


        [SerializeField, Min(0)]
        protected int m_magicRequired;
        protected Module m_module;

        protected virtual float CalculateMagicRequired() => m_magicRequired * m_module.modifier.magicRequirement;

        public virtual void Initialize(IPlayerModules player)
        {
            m_module = new Module(player);
        }

        public void DoSkill()
        {
            if (m_module.magic.currentValue < (CalculateMagicRequired()))
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
            m_module.magic.ReduceCurrentValue(Mathf.FloorToInt((CalculateMagicRequired())));
        }

        protected virtual void DenySkillUse()
        {

        }
    }
}