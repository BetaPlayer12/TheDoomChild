using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class ExplodingTrap : MonoBehaviour, ITrap
    {
        private DamagingExplosion m_damagingExplosion;
        private Collider2D m_trigger;

        public void TriggerTrap()
        {
            m_damagingExplosion.Explode();
            m_trigger.enabled = false;
        }

        private void Awake()
        {
            m_damagingExplosion = GetComponent<DamagingExplosion>();
            m_trigger = GetComponentInChildren<Collider2D>();
        }
    }
}