using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    [CreateAssetMenu(fileName = "IndividualLootData", menuName = "DChild/Gameplay/Loot/Individual Loot Data")]
    public class IndividualLootData : LootData
    {
        [SerializeField,ValidateInput("ValidateLoot","GameObject must have Loot component")]
        private GameObject m_loot;
        [SerializeField, MinValue(1)]
        private int m_count;

        public override void DropLoot(Vector2 position)
        {
            GameplaySystem.lootHandler.DropLoot(new LootDropRequest(m_loot, m_count, position));
        }
#if UNITY_EDITOR
        private bool ValidateLoot(GameObject loot)
        {
            return loot?.GetComponent<Loot>() ?? false;
        }
#endif
    }
}