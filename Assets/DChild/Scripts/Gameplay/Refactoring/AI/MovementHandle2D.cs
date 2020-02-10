using UnityEngine;

namespace Refactor.DChild.Gameplay
{
    public abstract class MovementHandle2D : MonoBehaviour
    {
        public abstract void MoveTowards(Vector2 direction, float speed);
        public abstract void Stop();
    }
}