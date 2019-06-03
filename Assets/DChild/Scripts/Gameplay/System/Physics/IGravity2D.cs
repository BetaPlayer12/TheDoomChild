using UnityEngine;

namespace DChild.Gameplay.Physics
{
    public interface IGravity2D
    {
        float gravityScale { get; set; }
        void SetAngle(Vector2 direction);
        void AddGravity(float scale);
    }
}
