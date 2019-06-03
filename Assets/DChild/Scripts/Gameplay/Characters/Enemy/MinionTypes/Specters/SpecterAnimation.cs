using System.Collections;
using UnityEngine;
using Spine.Unity;

namespace DChild.Gameplay.Characters.Enemies
{
    public abstract class SpecterAnimation : CombatCharacterAnimation
    {
        protected Coroutine m_flickerRoutine;
        private const float FLICKEROUTINE_INTERVAL = 0.25f;
        private float m_alphaLerpSpeed;

        public void Flicker()
        {
            StopAllCoroutines();
            if(m_flickerRoutine == null)
            {
                m_flickerRoutine = StartCoroutine(FlickerRoutine());
            }
        }

        public void StopFlicker()
        {
            if(m_flickerRoutine != null)
            {
                StopCoroutine(m_flickerRoutine);
            }

            StartCoroutine(FlickerRoutine());
        }

        private IEnumerator FlickerRoutine()
        {
            var skeleton = skeletonAnimation.skeleton;
            skeleton.SetColor(m_originalColor);

            Color currentSkinColor = m_originalColor;
            Color flickedColor = m_originalColor;
            flickedColor.a = 0;

            skeleton.SetColor(currentSkinColor);
            yield return null;

            while (true)
            {
                while (currentSkinColor != flickedColor)
                {
                    currentSkinColor.a -= Mathf.Clamp01(m_alphaLerpSpeed * Time.deltaTime);
                    skeleton.SetColor(currentSkinColor);
                    yield return null;
                }

                while (currentSkinColor != m_originalColor)
                {
                    currentSkinColor.a += Mathf.Clamp01(m_alphaLerpSpeed * Time.deltaTime);
                    skeleton.SetColor(currentSkinColor);
                    yield return null;
                }
            }
        }

        private IEnumerator StopFlickerRoutine()
        {
            var skeleton = skeletonAnimation.skeleton;
            Color currentSkinColor = skeleton.GetColor();

            while (currentSkinColor != m_originalColor)
            {
                currentSkinColor.a += Mathf.Clamp01(m_alphaLerpSpeed * Time.deltaTime);
                skeleton.SetColor(currentSkinColor);
                yield return null;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_alphaLerpSpeed = 1 / FLICKEROUTINE_INTERVAL;
        }

    }
}