using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChildEditor.Toolkit.EnemyCreation
{
    [System.Serializable]
    public class DamageableField
    {
        [SerializeField, MinValue(1)]
        private int m_maxHealth;
        [SerializeField]
        private bool m_useHealthCounter;

        public void Apply(GameObject instance, GameObject stats)
        {
            Health health = null;
            health = m_useHealthCounter ? (Health)stats.AddComponent<CounterHealth>() : stats.AddComponent<BasicHealth>();
            health.SetMaxValue(m_maxHealth);
            instance.AddComponent<Damageable>().InitializeField(null, health);
        }
    }
}