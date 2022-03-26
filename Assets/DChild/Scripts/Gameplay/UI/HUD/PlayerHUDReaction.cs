using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Combat;
using Doozy.Engine.UI;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    public class PlayerHUDReaction : MonoBehaviour
    {
        [SerializeField]
        private PlayerAvatar m_avatar;
        [SerializeField]
        private PlayerHUDReactionFX m_fx;
        [SerializeField]
        private UIView m_screenDamageFX;

        private Characters.Players.Modules.CharacterState m_state;
        private int m_shadowExecutionCount;
        private bool m_isEnrageActive;

        private Coroutine m_screenDamageRoutine;

        private void OnPlayerDamaged(object sender, Damageable.DamageEventArgs eventArgs)
        {
            m_avatar.ExecuteFlinch();

            if (m_screenDamageRoutine != null)
            {
                StopCoroutine(m_screenDamageRoutine);
            }

            m_screenDamageRoutine = StartCoroutine(ScreenFXRoutine());
        }

        private IEnumerator ScreenFXRoutine()
        {
            m_screenDamageFX.InstantHide();
            m_screenDamageFX.Show();

            while (m_screenDamageFX.IsShowing)
            {
                yield return null;
            }
            m_screenDamageFX.Hide();
        }

        private void OnShadowMorphEnd(object sender, EventActionArgs eventArgs)
        {
            m_shadowExecutionCount--;
            if (m_shadowExecutionCount == 0)
            {
                m_avatar.EndShadowMorph();
                m_fx.ShowShadowFX(false);
            }
            if (m_shadowExecutionCount <= 0)
            {
                m_shadowExecutionCount = 0;
            }
        }

        private void OnShadowMorphExecuted(object sender, EventActionArgs eventArgs)
        {
            if (m_shadowExecutionCount == 0)
            {
                m_avatar.ExecuteShadowMorph();
                m_fx.ShowShadowFX(true);
            }
            m_shadowExecutionCount++;
        }

        private void OnEnrageChange(object sender, EnrageEventArgs eventArgs)
        {
            if (eventArgs.value)
            {
                if (m_isEnrageActive == false)
                {
                    m_isEnrageActive = true;
                    if (m_shadowExecutionCount == 0)
                    {
                        m_avatar.ExecuteRage(true);
                    }
                    else
                    {
                        m_avatar.ExecuteRage(false);
                        m_avatar.ExecuteShadowMorph(false);
                    }
                    m_fx.ShowFireFX(true);
                }
            }
            else
            {
                if (m_isEnrageActive)
                {
                    m_isEnrageActive = false;
                    m_avatar.EndRage();
                    m_fx.ShowFireFX(false);
                }
            }
        }

        private void Start()
        {
            var player = GameplaySystem.playerManager.player;
            m_state = player.state;
            player.damageableModule.DamageTaken += OnPlayerDamaged;

            var shadowMorph = player.character.GetComponentInChildren<ShadowMorph>();
            shadowMorph.ExecuteModule += OnShadowMorphExecuted;
            shadowMorph.End += OnShadowMorphEnd;

            var shadowDash = player.character.GetComponentInChildren<ShadowDash>();
            shadowDash.ExecuteModule += OnShadowMorphExecuted;
            shadowDash.End += OnShadowMorphEnd;

            var shadowSlide = player.character.GetComponentInChildren<ShadowSlide>();
            shadowSlide.ExecuteModule += OnShadowMorphExecuted;
            shadowSlide.End += OnShadowMorphEnd;

            var devilWings = player.character.GetComponentInChildren<DevilWings>();
            devilWings.ExecuteModule += OnShadowMorphExecuted;
            devilWings.End += OnShadowMorphEnd;

            player.state.EnrageChange += OnEnrageChange;

            m_avatar.ExecuteIdle();
            m_fx.HideAll();
        }
    }
}