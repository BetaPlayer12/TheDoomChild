using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public interface IGrappleObject
    {
        IsolatedPhysics2D physics { get; }
        Vector2 position { get; }
        bool canBePulled { get; }
        float pullOffset { get; }
        float dashOffset { get; }
    }
}