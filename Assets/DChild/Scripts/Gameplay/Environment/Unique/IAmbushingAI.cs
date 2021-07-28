using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public interface IAmbushingAI
    {
        GameObject gameObject { get; }
        void Ambush(Vector2 position);
    }
}