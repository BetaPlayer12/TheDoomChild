using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Combat;
using Spine.Unity;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    public class PlayerAvatar : MonoBehaviour
    {
        [System.Serializable]
        private class ModeChangeInfo
        {
            [SerializeField]
            private AnimationReferenceAsset m_startAnimation;
            [SerializeField]
            private AnimationReferenceAsset m_loopAnimation;

            public AnimationReferenceAsset startAnimation => m_startAnimation;
            public AnimationReferenceAsset loopAnimation => m_loopAnimation;
        }

        [SerializeField]
        private AnimationReferenceAsset m_normalIdle;
        [SerializeField]
        private AnimationReferenceAsset m_flinch;

        private SkeletonGraphic m_animation;
        private CharacterState m_state;

        private void Start()
        {
            m_animation = GetComponent<SkeletonGraphic>();

            var player = GameplaySystem.playerManager.player;
            m_state = player.state;
            player.damageableModule.DamageTaken += OnPlayerDamaged;

            m_animation.AnimationState.SetAnimation(0, m_normalIdle, true);
        }

        private void OnPlayerDamaged(object sender, Damageable.DamageEventArgs eventArgs)
        {
            m_animation.AnimationState.SetAnimation(0, m_flinch, false);
            m_animation.AnimationState.AddAnimation(0, m_normalIdle, true,0);
        }
    }
}