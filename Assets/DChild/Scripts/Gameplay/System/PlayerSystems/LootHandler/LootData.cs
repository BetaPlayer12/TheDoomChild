using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Systems
{

    [CreateAssetMenu(fileName = "LootData", menuName = "DChild/Gameplay/Loot/Loot Data")]
    public class LootData : SerializedScriptableObject
    {
        [SerializeField]
        private ILootDataContainer m_lootData;
        public void DropLoot(Vector2 position) => m_lootData.DropLoot(position);
    }
}