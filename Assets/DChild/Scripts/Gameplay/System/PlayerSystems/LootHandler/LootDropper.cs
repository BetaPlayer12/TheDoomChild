using Holysoft.Event;
using DChild.Gameplay.Combat;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.Systems
{
    public class LootDropper : MonoBehaviour
    {
        [SerializeField]
        private LootData m_loot;
        [SerializeField]
        private bool m_dropWhenDestroyed;
        private Damageable m_damageable;

        [Button]
        public void DropLoot()
        {
            m_loot.DropLoot(m_damageable.position);
        }

        public void SetLootData(LootData lootData)
        {
            m_loot = lootData;
        }

        private void Awake()
        {
            m_damageable = GetComponentInParent<Damageable>();
            if (m_dropWhenDestroyed)
            {
                m_damageable.Destroyed += OnDestroyed;
            }
        }

        private void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            DropLoot();
        }
    }
}