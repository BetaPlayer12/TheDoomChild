using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public interface IAmbushingAI
    {
        GameObject gameObject { get; }
        void LaunchAmbush(Vector2 position);
        void PrepareAmbush(Vector2 position);
    }
}