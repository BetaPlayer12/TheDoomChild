using DChild.Gameplay.Systems.WorldComponents;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusAilment
{
    public struct StatusEffectRecieverEventArgs : IEventActionArgs
    {
        public StatusEffectRecieverEventArgs(StatusEffectType type) : this()
        {
            this.type = type;
        }

        public StatusEffectType type { get; }
    }

    [AddComponentMenu("DChild/Gameplay/Combat/Status Effect Reciever")]
    public class StatusEffectReciever : MonoBehaviour
    {
        [SerializeField]
        private Character m_character;
        [SerializeField]
        private StatusEffectResistance m_resistance;

        private IsolatedObject m_isolatedObject;
        [ShowInInspector, ReadOnly, HideInEditorMode]
        private List<StatusEffectHandle> m_inflictedStatusEffects;

        public IStatusEffectResistance resistance => m_resistance;

        public event EventAction<StatusEffectRecieverEventArgs> StatusRecieved;
        public event EventAction<StatusEffectRecieverEventArgs> StatusEnd;


        public void RecieveStatusEffect(StatusEffectHandle statusEffect)
        {
            statusEffect.Initialize(m_character);
            statusEffect.StartEffect();
            statusEffect.DurationExpired += OnEffectEnd;
            m_inflictedStatusEffects.Add(statusEffect);
            StatusRecieved?.Invoke(this, new StatusEffectRecieverEventArgs(statusEffect.type));
        }

        public bool IsInflictedWith(StatusEffectType type) => Contains(type, out int index);

        public float GetCurrentDurationOf(StatusEffectType type)
        {
            if (Contains(type, out int index))
            {
                return m_inflictedStatusEffects[index].duration;
            }
            else
            {
                return 0;
            }
        }

        public void SetCurrentDurationOf(StatusEffectType type, float duration)
        {
            if (Contains(type, out int index))
            {
                m_inflictedStatusEffects[index].duration = duration;
            }
        }

        public void ResetDuration(StatusEffectType type)
        {
            if (Contains(type, out int index))
            {
                m_inflictedStatusEffects[index].ResetDuration();
            }
        }

        public void StopStatusEffect(StatusEffectType type)
        {
            if (Contains(type, out int index))
            {
                m_inflictedStatusEffects[index].StopEffect();
                m_inflictedStatusEffects.RemoveAt(index);
                StatusEnd?.Invoke(this, new StatusEffectRecieverEventArgs(type));
            }
        }

        public void SetActive(StatusEffectType type, bool active)
        {
            if (Contains(type, out int index))
            {
                m_inflictedStatusEffects[index].isActive = active;
            }
        }

        public void EnableUpdatableEffects()
        {
            for (int i = 0; i < m_inflictedStatusEffects.Count; i++)
            {
                m_inflictedStatusEffects[i].EnableModules();
            }
        }

        public void DisableUpdatableEffects()
        {
            for (int i = 0; i < m_inflictedStatusEffects.Count; i++)
            {
                m_inflictedStatusEffects[i].DisableModules();
            }
        }

        public void RemoveAllActiveStatusEffects()
        {
            foreach (var statusEffect in m_inflictedStatusEffects)
            {
                statusEffect.StopEffect();
                StatusEnd?.Invoke(this, new StatusEffectRecieverEventArgs(statusEffect.type));
            }

            m_inflictedStatusEffects.Clear();
        }

        private bool Contains(StatusEffectType type, out int index)
        {
            for (int i = 0; i < m_inflictedStatusEffects.Count; i++)
            {
                if (m_inflictedStatusEffects[i].type == type)
                {
                    index = i;
                    return true;
                }
            }
            index = -1;
            return false;
        }

        private void OnEffectEnd(object sender, StatusEffectReferenceEventArgs eventArgs)
        {
            eventArgs.statusEffect.StopEffect();
            m_inflictedStatusEffects.Remove(eventArgs.statusEffect);
            StatusEnd?.Invoke(this, new StatusEffectRecieverEventArgs(eventArgs.statusEffect.type));
        }

        private void Awake()
        {
            m_inflictedStatusEffects = new List<StatusEffectHandle>();
            m_isolatedObject = m_character.isolatedObject;
        }

        private void Update()
        {
            if (m_inflictedStatusEffects.Count > 0)
            {
                var deltaTime = m_isolatedObject?.deltaTime ?? Time.deltaTime;
                for (int i = 0; i < m_inflictedStatusEffects.Count; i++)
                {
                    m_inflictedStatusEffects[i].Update(deltaTime);
                }
            }
        }

#if UNITY_EDITOR
        public void InitializeField(Character character)
        {
            m_character = character;
        }

        public void InitializeField(StatusEffectResistance resistance)
        {
            m_resistance = resistance;
        }
#endif
    }
}