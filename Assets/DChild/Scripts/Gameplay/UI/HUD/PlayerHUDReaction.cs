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

        private void OnPlayerDamaged(object sender, Damageable.DamageEventArgs eventArgs)
        {
            m_avatar.ExecuteFlinch();
        }

        private void OnShadowMorphEnd(object sender, EventActionArgs eventArgs)
        {
            m_avatar.EndShadowMorph();
            m_fx.ShowShadowFX(false);

        }

        private void OnShadowMorphExecuted(object sender, EventActionArgs eventArgs)
        {
            m_avatar.ExecuteShadowMorph();
            m_fx.ShowShadowFX(true);
        }

        private void Start()
        {
            var player = GameplaySystem.playerManager.player;
            m_state = player.state;
            player.damageableModule.DamageTaken += OnPlayerDamaged;

            var shadowMorph = player.character.GetComponentInChildren<ShadowMorph>();
            shadowMorph.ExecuteModule += OnShadowMorphExecuted;
            shadowMorph.End += OnShadowMorphEnd;

            m_avatar.ExecuteIdle();
            m_fx.HideAll();
        }
    }
}