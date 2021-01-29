using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    public class PlayerHUDReaction : MonoBehaviour
    {
        [SerializeField]
        private PlayerAvatar m_avatar;
        [SerializeField]
        private PlayerHUDReactionFX m_fx;

        private CharacterState m_state;
        private int m_shadowExecutionCount;

        private void OnPlayerDamaged(object sender, Damageable.DamageEventArgs eventArgs)
        {
            m_avatar.ExecuteFlinch();
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

        private void Start()
        {
            var player = GameplaySystem.playerManager.player;
            m_state = player.state;
            player.damageableModule.DamageTaken += OnPlayerDamaged;

            var shadowMorph = player.character.GetComponentInChildren<ShadowMorph>();
            shadowMorph.ExecuteShadowMorph += OnShadowMorphExecuted;
            shadowMorph.EndShadowMorphExecution += OnShadowMorphEnd;

            var shadowDash = player.character.GetComponentInChildren<ShadowDash>();
            shadowDash.ExecuteShadowDash += OnShadowMorphExecuted;
            shadowDash.EndShadowDashExecution += OnShadowMorphEnd;

            var shadowSlide = player.character.GetComponentInChildren<ShadowSlide>();
            shadowSlide.ExecuteShadowSlide += OnShadowMorphExecuted;
            shadowSlide.EndShadowSlideExecution += OnShadowMorphEnd;

            var devilWings = player.character.GetComponentInChildren<DevilWings>();
            devilWings.ExecuteDevilWings += OnShadowMorphExecuted;
            devilWings.EndDevilWingsExecution += OnShadowMorphEnd;

            m_avatar.ExecuteIdle();
            m_fx.HideAll();
        }
    }
}