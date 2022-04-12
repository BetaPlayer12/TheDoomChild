using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public interface ILootDataContainer
    {
        void DropLoot(Vector2 position);

        //void DropLootAndRecord(Vector2 position, ref LootList recordList);

        void GenerateLootInfo(ref LootList recordList);

#if UNITY_EDITOR
        void DrawDetails(bool drawContainer, string label = null);
#endif
    }
}