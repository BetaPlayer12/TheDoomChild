using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public class PlayerModuleActivator : MonoBehaviour
    {
        public enum Module
        {
            [HideInInspector]
            _COUNT
        }

        public struct UpdateEventArgs : IEventActionArgs
        {
            public UpdateEventArgs(Module module, bool isEnabled) : this()
            {
                this.module = module;
                isPrimarySkill = false;
                this.isEnabled = isEnabled;
            }

            public UpdateEventArgs(PrimarySkill skill, bool isEnabled) : this()
            {
                this.skill = skill;
                isPrimarySkill = true;
                this.isEnabled = isEnabled;
            }

            public Module module { get; }
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
        [ShowInInspector]
        private Dictionary<Module, State> m_statePair;
        [ShowInInspector]
        private Dictionary<PrimarySkill, State> m_skillPair;

        public event EventAction<UpdateEventArgs> OnUpdate;

        public void SetModuleActive(Module module, bool isActive)
        {
            var state = m_statePair[module];
            state.active = isActive;
            m_statePair[module] = state;
            EndUpdate(module);
        }

        public void SetModuleLock(Module module, bool isUnlocked)
        {
            var state = m_statePair[module];
            state.active = isUnlocked;
            m_statePair[module] = state;
            EndUpdate(module);
        }

        public bool IsModuleActive(Module module)
        {
            var state = m_statePair[module];
            return state.unlocked && state.active;
        }

        public void SetModuleActive(PrimarySkill module, bool isActive)
        {
            var state = m_skillPair[module];
            state.active = isActive;
            m_skillPair[module] = state;
            EndUpdate(module);
            //Do this incase the thing is actually a passive module
            m_passiveModule.SetModuleActive(module, isActive); 
        }

        public void SetModuleLock(PrimarySkill module, bool isUnlocked)
        {
            var state = m_skillPair[module];
            state.active = isUnlocked;
            m_skillPair[module] = state;
            EndUpdate(module);
        }

        public bool IsModuleActive(PrimarySkill module)
        {
            var state = m_skillPair[module];
            return state.unlocked && state.active;
        }

        public void Validate()
        {
            if (m_statePair == null)
            {
                m_statePair = new Dictionary<Module, State>();
                for (int i = 0; i < (int)Module._COUNT; i++)
                {
                    var module = (Module)i;
                    m_statePair.Add(module, new State(true, true));
                    m_passiveModule.SetModuleActive(module, true);
                }
            }

            if (m_skillPair == null)
            {
                m_skillPair = new Dictionary<PrimarySkill, State>();
                for (int i = 0; i < (int)PrimarySkill._COUNT; i++)
                {
                    var skill = (PrimarySkill)i;
                    m_skillPair.Add(skill, new State(true, true));
                    m_passiveModule.SetModuleActive(skill, true);
                }
            }
        }

        private void EndUpdate(Module module)
        {
            var isActive = IsModuleActive(module);
            m_passiveModule.SetModuleActive(module, isActive);
            OnUpdate?.Invoke(this, new UpdateEventArgs(module, isActive));
        }

        private void EndUpdate(PrimarySkill module)
        {
            var isActive = IsModuleActive(module);
            m_passiveModule.SetModuleActive(module, isActive);
            OnUpdate?.Invoke(this, new UpdateEventArgs(module, isActive));
        }

        public void Reset()
        {
            if (m_statePair != null)
            {
                for (int i = 0; i < (int)Module._COUNT; i++)
                {
                    SetModuleActive((Module)i, true);
                }
            }

            if (m_skillPair != null)
            {
                for (int i = 0; i < (int)PrimarySkill._COUNT; i++)
                {
                    SetModuleActive((PrimarySkill)i, true);
                }
            }
        }

        private void Awake()
        {
            Validate();
        }
    }
}