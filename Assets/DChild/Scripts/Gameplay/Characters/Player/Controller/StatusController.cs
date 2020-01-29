using System;
using System.Collections.Generic;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Combat.StatusAilment;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    [AddComponentMenu("DChild/Gameplay/Player/Controller/Status Controller")]
    public class StatusController : MonoBehaviour
    {
        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private FrozenBreak m_frozenBreak;
        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private BleedingCrouch m_bleedingCrouch;

        private bool m_isActive;
        private List<StatusEffectType> m_inflictedStatusEffects;

        public bool isActive => m_isActive;

        public void Initialize(GameObject behaviours, StatusEffectReciever reciever)
        {
            m_inflictedStatusEffects = new List<StatusEffectType>();
            m_frozenBreak = behaviours.GetComponentInChildren<FrozenBreak>();
            m_bleedingCrouch = behaviours.GetComponentInChildren<BleedingCrouch>();
            reciever.StatusRecieved += OnStatusRecieved;
            reciever.StatusEnd += OnStatusEnd;
        }

        public void CallUpdate(IPlayerState state, ControllerEventArgs eventArgs)
        {
            if (m_inflictedStatusEffects.Contains(StatusEffectType.Frozen))
            {
                m_frozenBreak.HandleBreak(eventArgs.input.direction);
            }
            if (m_inflictedStatusEffects.Contains(StatusEffectType.Bleeding))
            {
                m_bleedingCrouch.Handle(eventArgs.input.direction);
            }
        }

        private void OnStatusEnd(object sender, StatusEffectRecieverEventArgs eventArgs)
        {
            m_inflictedStatusEffects.Remove(eventArgs.type);
            m_isActive = m_inflictedStatusEffects.Count > 0;
        }

        private void OnStatusRecieved(object sender, StatusEffectRecieverEventArgs eventArgs)
        {
            m_inflictedStatusEffects.Add(eventArgs.type);
            m_isActive = m_inflictedStatusEffects.Count > 0;
        }
    }
}