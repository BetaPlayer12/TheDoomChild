using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class PuedisYnnusMultipleSpikeHandle : MonoBehaviour
    {
        [SerializeField]
        private Transform m_spikes;
        [SerializeField]
        private Vector2 m_spikesDisappearPosition;
        [SerializeField]
        private Vector2 m_spikesGrowPosition;
        [SerializeField]
        private float m_growTransistionDuration;
        [SerializeField]
        private float m_disappearTransistionDuration;
        [SerializeField]
        private ParticleSystem m_dustFX;

        [Button]
        public void Grow()
        {
            StopAllCoroutines();
            StartCoroutine(TransistionRoutine(m_spikesDisappearPosition, m_spikesGrowPosition, m_growTransistionDuration));
        }

        [Button]
        public void Disappear()
        {
            StopAllCoroutines();
            StartCoroutine(DisappearRoutine());
        }

        private IEnumerator DisappearRoutine()
        {
            yield return TransistionRoutine(m_spikesGrowPosition, m_spikesDisappearPosition, m_disappearTransistionDuration);
            m_spikes.gameObject.SetActive(false);
        }

        private IEnumerator TransistionRoutine(Vector2 fromPosition, Vector2 toPosition, float duration)
        {
            m_dustFX.Play(true);
            var lerpSpeed = 1f / duration;
            float lerpValue = 0;
            m_spikes.localPosition = fromPosition;
            m_spikes.gameObject.SetActive(true);
            do
            {
                lerpValue += lerpSpeed * GameplaySystem.time.deltaTime;
                m_spikes.localPosition = Vector3.Lerp(fromPosition, toPosition, lerpValue);
                yield return null;
            } while (lerpValue < 1);
            m_dustFX.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        private void Awake()
        {
            m_spikes.gameObject.SetActive(false);
        }
    }
}