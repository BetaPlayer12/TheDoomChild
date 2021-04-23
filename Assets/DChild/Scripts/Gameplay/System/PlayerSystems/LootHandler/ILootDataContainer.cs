using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public interface ILootDataContainer
    {
        void DropLoot(Vector2 position);

#if UNITY_EDITOR
        void DrawDetails(bool drawContainer, string label = null);
#endif
    }
}