using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class PuedisYnnusSequenceMassiveSpikePattern : SerializedMonoBehaviour
    {
        [SerializeField]
        private float m_nextIndexInterval;
        [SerializeField]
        private float m_spikeDuration;
        [SerializeField]
        private List<PuedisYnnusSpike[]> m_spikeSequence;

        [Button]
        private void TestSequence()
        {
            StartCoroutine(ExecuteSequence());
        }

        public IEnumerator ExecuteSequence()
        {
            for (int i = 0; i < m_spikeSequence.Count; i++)
            {
                StartCoroutine(ShowSpike(m_spikeSequence[i]));
                yield return new WaitForSeconds(m_nextIndexInterval);
            }
        }


        private IEnumerator ShowSpike(PuedisYnnusSpike[] spikes)
        {
            for (int i = 0; i < spikes.Length; i++)
            {
                spikes[i].Grow();
            }

            yield return new WaitForSeconds(m_spikeDuration);

            for (int i = 0; i < spikes.Length; i++)
            {
                spikes[i].Disappear();
            }
        }
    }
}