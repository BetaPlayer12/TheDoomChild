using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class PuedisYnnusMultipleSpikeHandle : MonoBehaviour
    {
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