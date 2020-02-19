using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public abstract class LootData : ScriptableObject
    {
        public abstract void DropLoot(Vector2 position);
    }
}