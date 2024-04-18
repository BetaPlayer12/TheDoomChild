using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class PuedisYnnusMultipleSpikeHandle : MonoBehaviour
    {
        [SerializeField]
        private Transform m_spikes;
        [SerializeField]
        private Vector2 m_spikesHiddenPosition;
        [SerializeField]
        private Vector2 m_spikesrevealed;
        [SerializeField]
        private ParticleSystem m_dustFX;

        public void Grow()
        {
            gameObject.SetActive(true);
            Debug.Log(gameObject.name);
        }

        public void Disappear()
        {
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}