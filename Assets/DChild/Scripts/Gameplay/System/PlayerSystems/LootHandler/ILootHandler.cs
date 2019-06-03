using DChild.Gameplay.SoulEssence;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public interface ILootHandler
    {
        void Drop(SoulEssenceDropInfo info, Vector2 position);
    }
}