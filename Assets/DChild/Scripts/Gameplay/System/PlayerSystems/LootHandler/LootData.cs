using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Systems
{

    [CreateAssetMenu(fileName = "LootData", menuName = "DChild/Gameplay/Loot/Loot Data")]
    public class LootData : SerializedScriptableObject, ILootDataContainer
    {
        [SerializeField,LabelWidth(100)]
        private ILootDataContainer m_data;
        public void DropLoot(Vector2 position) => m_data.DropLoot(position);

        public void GenerateLootInfo(ref LootList recordList)
        {
            m_data.GenerateLootInfo(ref recordList);
        }

#if UNITY_EDITOR
        public void DrawDetails(bool drawContainer, string label = null)
        {
            m_data.DrawDetails(drawContainer, label);
        }
        public ILootDataContainer data => m_data;
#endif
    }
}