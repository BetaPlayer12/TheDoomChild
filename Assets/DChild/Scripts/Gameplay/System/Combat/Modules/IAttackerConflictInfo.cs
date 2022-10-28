using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public interface IAttackerConflictInfo
    {
        bool isPlayer { get; }
        GameObject instance { get; }
        Vector2 position { get; }
    }
}