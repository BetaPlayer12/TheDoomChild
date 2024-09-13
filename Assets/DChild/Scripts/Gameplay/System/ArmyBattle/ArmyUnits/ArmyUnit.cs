using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using USpine = Spine.Unity;

namespace DChild.Gameplay.ArmyBattle.Units
{
    [DisallowMultipleComponent]
    public abstract class ArmyUnit : MonoBehaviour
    {
        [SerializeField]
        protected SkeletonAnimation m_animation;

        [SerializeField, USpine.SpineAnimation]
        private string[] m_idleAnimations;
        [SerializeField, USpine.SpineAnimation]
        private string[] m_attackAnimations;
        [SerializeField, USpine.SpineAnimation]
        private string[] m_deathAnimations;

        public abstract DamageType type { get; }

        [Button]
        public virtual void Attack()
        {
            PlayRandomAnimation(m_attackAnimations, true);
        }

        [Button]
        public virtual void Die()
        {
            PlayRandomAnimation(m_deathAnimations, false);
        }

        [Button]
        public virtual void Idle()
        {
            PlayRandomAnimation(m_idleAnimations, true);
        }

        private void PlayRandomAnimation(string[] animations, bool loop)
        {
            var animation = ChooseAnimation(animations);

            var track = m_animation.state.SetAnimation(0, animation, loop);
            if (loop)
            {
                var endAnimation = track.AnimationEnd * 0.3f;
                var startingTimeStart = Mathf.Lerp(track.AnimationStart, endAnimation, Random.Range(0f, 1f));
                track.AnimationStart = startingTimeStart;
            }
        }

        private string ChooseAnimation(string[] animations)
        {
            var animationIndex = Random.Range(0, animations.Length);
            return animations[animationIndex];
        }
    }
}