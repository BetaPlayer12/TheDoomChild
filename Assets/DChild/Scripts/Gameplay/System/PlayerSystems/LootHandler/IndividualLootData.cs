using UnityEngine;

namespace DChild.Gameplay.Systems
{
    [System.Serializable]
    public class IndividualLootData : ILootDataContainer
    {
        [SerializeField]
        private LootReference m_reference;
        [SerializeField, Min(1)]
        private int m_count = 1;

        public void DropLoot(Vector2 position)
        {
            GameplaySystem.lootHandler.DropLoot(new LootDropRequest(m_reference.loot, m_count, position));
        }
    }
}