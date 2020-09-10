using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Systems
{

    [CreateAssetMenu(fileName = "LootData", menuName = "DChild/Gameplay/Loot/Loot Data")]
    public class LootData : SerializedScriptableObject
    {
        [SerializeField,LabelWidth(100)]
        private ILootDataContainer m_data;
        public void DropLoot(Vector2 position) => m_data.DropLoot(position);

#if UNITY_EDITOR
        public ILootDataContainer data => m_data;
#endif
    }
}