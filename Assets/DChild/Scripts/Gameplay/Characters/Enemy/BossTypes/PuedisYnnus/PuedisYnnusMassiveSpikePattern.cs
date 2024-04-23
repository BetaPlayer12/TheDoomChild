using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{

    public class PuedisYnnusMassiveSpikePattern : MonoBehaviour
    {
        [SerializeField]
        private PuedisYnnusSpike[] m_spikes;

        public void Grow()
        {
            gameObject.SetActive(true);
            for (int i = 0; i < m_spikes.Length; i++)
            {
                m_spikes[i].Grow();
            }
        }

        public void Disappear()
        {
            for (int i = 0; i < m_spikes.Length; i++)
            {
                m_spikes[i].Disappear();
            }
        }

        private void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}