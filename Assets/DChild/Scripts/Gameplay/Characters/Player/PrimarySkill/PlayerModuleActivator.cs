﻿using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public class PlayerModuleActivator : MonoBehaviour
    {
        public struct UpdateEventArgs : IEventActionArgs
        {
            public UpdateEventArgs(PrimarySkill skill, bool isEnabled) : this()
            {
                this.skill = skill;
                isPrimarySkill = true;
                this.isEnabled = isEnabled;
            }
            public PrimarySkill skill { get; }

            public bool isPrimarySkill { get; }
            public bool isEnabled { get; }
        }

        private struct State
        {
            public bool unlocked;
            public bool active;

            public State(bool unlocked, bool active)
            {
                this.unlocked = unlocked;
                this.active = active;
            }
        }

        [SerializeField]
        private PlayerPassiveModule m_passiveModule;
        [ShowInInspector,ReadOnly]
        private PrimarySkill m_unlockedSkills;
        [ShowInInspector, ReadOnly]
        private PrimarySkill m_activatedSkills;

        public event EventAction<UpdateEventArgs> OnUpdate;

        public void SetModuleActive(PrimarySkill module, bool isActive)
        {
            var state = m_activatedSkills.HasFlag(module);

            if (isActive)
            {
                m_activatedSkills |= module;
            }
            else
            {
                m_activatedSkills &= ~module;
            }

            EndUpdate(module);
            //Do this incase the thing is actually a passive module
            m_passiveModule.SetModuleActive(module, isActive);
        }

        public void SetModuleLock(PrimarySkill module, bool isUnlocked)
        {
            if (isUnlocked)
            {
                m_unlockedSkills |= module;
            }
            else
            {
                m_unlockedSkills &= ~module;
            }
            EndUpdate(module);
        }

        public bool IsModuleActive(PrimarySkill module)
        {
            return m_unlockedSkills.HasFlag(module) && m_activatedSkills.HasFlag(module);
        }

        public void Validate()
        {
            m_activatedSkills = PrimarySkill.All;
            m_unlockedSkills = PrimarySkill.All;
        }

        private void EndUpdate(PrimarySkill module)
        {
            var isActive = IsModuleActive(module);
            m_passiveModule.SetModuleActive(module, isActive);
            OnUpdate?.Invoke(this, new UpdateEventArgs(module, isActive));
        }

        public void Reset()
        {
            m_activatedSkills = PrimarySkill.All;
        }

        private void Awake()
        {
            Validate();
        }
    }
}