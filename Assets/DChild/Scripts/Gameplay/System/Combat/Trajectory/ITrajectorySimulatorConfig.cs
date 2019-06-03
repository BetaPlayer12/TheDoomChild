using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public interface ITrajectorySimulatorConfig
    {
        void SetObjectValues(float mass,float gravityScale);
        void SetObjectValues(float mass,float gravityScale, BoxCollider2D collider2D);
        void SetObjectValues(float mass,float gravityScale, CircleCollider2D collider2D);
        void SetObjectValues(float mass,float gravityScale, CapsuleCollider2D collider2D);
        void SetVelocity(Vector2 velocity);
        void SetStartPosition(Vector2 startPosition);
    }
}