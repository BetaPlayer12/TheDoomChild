using DChild.Gameplay.Combat.StatusAilment;
using Holysoft.Event;
using System;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class SnaredBreak : MonoBehaviour, IComplexCharacterModule
    {
        [SerializeField]
        private BasicSlashes.Type[] m_validBasicSlashes;
        [SerializeField]
        private WhipAttack.Type[] m_validWhipAttack;

        private StatusEffectReciever m_statusEffectReciever;
        private BasicSlashes m_basicSlashes;
        private WhipAttack m_whisperAttack;
        private SlashCombo m_slashCombo;
        private SwordThrust m_swordThrust;
        private ShadowMorph m_shadowMorph;
        private ShadowDash m_shadowDash;
        private ShadowSlide m_shadowSlide;

        private bool m_isActive;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_statusEffectReciever = info.statusEffectReciever;
            m_statusEffectReciever.StatusRecieved += OnStatusEffectRecieved;

            var character = info.character;
            m_basicSlashes = character.GetComponentInChildren<BasicSlashes>();
            m_whisperAttack = character.GetComponentInChildren<WhipAttack>();
            m_slashCombo = character.GetComponentInChildren<SlashCombo>();
            m_swordThrust = character.GetComponentInChildren<SwordThrust>();
            m_shadowMorph = character.GetComponentInChildren<ShadowMorph>();
            m_shadowDash = character.GetComponentInChildren<ShadowDash>();
            m_shadowSlide = character.GetComponentInChildren<ShadowSlide>();

        }

        private void OnStatusEffectRecieved(object sender, StatusEffectRecieverEventArgs eventArgs)
        {
            if (m_isActive)
                return;

            if (eventArgs.type == StatusEffectType.Snared)
            {
                m_isActive = true;
                m_basicSlashes.OnSlash += OnSlash;
                m_whisperAttack.OnWhip += OnWhip;
                m_slashCombo.OnSlash += OnExecuteBehaviour;
                m_swordThrust.OnThrust += OnExecuteBehaviour;
                m_shadowMorph.ExecuteModule += OnExecuteBehaviour;
                m_shadowDash.ExecuteModule += OnExecuteBehaviour;
                m_shadowSlide.ExecuteModule += OnExecuteBehaviour;
                m_statusEffectReciever.StatusEnd += OnStatusEnd;
            }
        }

        private void DeactivateSelf()
        {
            m_isActive = false;
            m_basicSlashes.OnSlash -= OnSlash;
            m_whisperAttack.OnWhip -= OnWhip;
            m_slashCombo.OnSlash -= OnExecuteBehaviour;
            m_swordThrust.OnThrust -= OnExecuteBehaviour;
            m_shadowMorph.ExecuteModule -= OnExecuteBehaviour;
            m_shadowDash.ExecuteModule -= OnExecuteBehaviour;
            m_shadowSlide.ExecuteModule -= OnExecuteBehaviour;
            m_statusEffectReciever.StatusEnd -= OnStatusEnd;
        }

        private void OnStatusEnd(object sender, StatusEffectRecieverEventArgs eventArgs)
        {
            DeactivateSelf();
        }

        private void OnExecuteBehaviour(object sender, EventActionArgs eventArgs)
        {
            if (m_isActive)
            {
                CureStatusAilment();
            }
        }

        private void OnSlash(object sender, BasicSlashes.BasicSlashEventArgs eventArgs)
        {
            if (m_isActive)
            {
                bool isValid = false;
                for (int i = 0; i < m_validBasicSlashes.Length; i++)
                {
                    if (m_validBasicSlashes[i] == eventArgs.type)
                    {
                        isValid = true;
                        break;
                    }
                }

                if (isValid)
                {
                    CureStatusAilment();
                }
            }
            
        }

        private void OnWhip(object sender, WhipAttack.WhipAttackEventArgs eventArgs)
        {
            if (m_isActive)
            {
                bool isValid = false;
                for (int i = 0; i < m_validWhipAttack.Length; i++)
                {
                    if (m_validWhipAttack[i] == eventArgs.type)
                    {
                        isValid = true;
                        break;
                    }
                }

                if (isValid)
                {
                    CureStatusAilment();
                }
            }
        }

        private void CureStatusAilment()
        {
            m_statusEffectReciever.StopStatusEffect(StatusEffectType.Snared);
        }
    }
}