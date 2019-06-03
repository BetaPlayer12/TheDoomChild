/***********************************************
 * 
 * Base Animation class for All Character Animations
 * Should Contain functions that will be present to all
 * Types of Characters
 * 
 * Child Classess should have functions that does not care about 
 * Transistion from one state to another
 * 
 ***********************************************/

using System.Collections;
using Spine.Unity;
using Spine.Unity.Modules;
using UnityEngine;

namespace DChild.Gameplay.Characters
{

    public abstract class CombatCharacterAnimation : SpineRootAnimation
    {
        public const string ANIMATION_IDLE = "Idle";
        public const string ANIMATION_DEATH = "Death";

        protected Coroutine m_damageRoutine;
        protected Color m_originalColor;
        private const float DAMAGEROUTINE_DURATION = 0.25f;
        private Color m_damageColor;
        private Color m_colorLerpSpeed;

        public virtual void DoIdle() => SetAnimation(0, ANIMATION_IDLE, true);
        public virtual void DoDeath() => SetAnimation(0, ANIMATION_DEATH, false);     

        public void DoDamage()
        {
            StopAllCoroutines();
            m_damageRoutine = StartCoroutine(DamageRoutine());
        }

        protected override void Awake()
        {
            base.Awake();
            m_damageColor = Color.red;
            m_originalColor = Color.white;
            m_colorLerpSpeed.r = (m_originalColor.r - m_damageColor.r) / DAMAGEROUTINE_DURATION;
            m_colorLerpSpeed.g = (m_originalColor.g - m_damageColor.g) / DAMAGEROUTINE_DURATION;
            m_colorLerpSpeed.b = (m_originalColor.b - m_damageColor.b) / DAMAGEROUTINE_DURATION;
            m_colorLerpSpeed.a = (m_originalColor.a - m_damageColor.a) / DAMAGEROUTINE_DURATION;
        }

        private IEnumerator DamageRoutine()
        {
            Color currentSkinColor = m_damageColor;

            var skeleton = skeletonAnimation.skeleton;
            skeleton.SetColor(currentSkinColor);
            yield return null;

            while (currentSkinColor != m_originalColor)
            {
                currentSkinColor += m_colorLerpSpeed * Time.deltaTime;
                currentSkinColor.r = Mathf.Clamp01(currentSkinColor.r);
                currentSkinColor.g = Mathf.Clamp01(currentSkinColor.g);
                currentSkinColor.b = Mathf.Clamp01(currentSkinColor.b);
                currentSkinColor.a = Mathf.Clamp01(currentSkinColor.a);
                skeleton.SetColor(currentSkinColor);
                yield return null;
            }

            m_damageRoutine = null;
        }
    }
}