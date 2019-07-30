using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

namespace DChild.Gameplay.Characters.Enemies
{
    public class HorseSeaFX : FX
    {
        #region "Animation Names"
        public const string ANIMATION_BEFORE_LOOP = "BeforeLoop";
        public const string ANIMATION_LOOP = "Projectile_Loop";
        public const string ANIMATION_EXPLODE = "Projectile_Explode";
        #endregion

        [SerializeField]
        private bool m_isExploding;

        private SkeletonAnimation m_animation;

        public void DoProjectile()
        {
            //m_animation.AnimationState.SetAnimation(0, ANIMATION_BEFORE_LOOP, false);
            m_animation.AnimationState.AddAnimation(0, ANIMATION_LOOP, true, 0);
        }

        public void DoExplode()
        {
            m_animation.AnimationState.SetAnimation(0, ANIMATION_EXPLODE, false);
        }

        public void DoExplodeRoutine()
        {
            StartCoroutine(DetonateRoutine());
        }

        private IEnumerator DetonateRoutine()
        {
            DoExplode();
            yield return new WaitForAnimationComplete(m_animation.AnimationState, HorseSeaFX.ANIMATION_EXPLODE);
            Destroy(this.gameObject);
        }

        private void Awake()
        {
            m_animation = GetComponentInChildren<SkeletonAnimation>();
        }

        public override void Play()
        {
            if (!m_isExploding)
            {
                DoProjectile();
            }
            else
            {
                DoExplode();
            }
        }

        public override void SetFacing(HorizontalDirection horizontalDirection)
        {
            throw new System.NotImplementedException();
        }

        public override void Stop()
        {
            throw new System.NotImplementedException();
        }

        public override void Pause()
        {
            throw new System.NotImplementedException();
        }
    }
}
