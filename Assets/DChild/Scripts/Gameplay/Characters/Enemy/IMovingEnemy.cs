using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public interface IMovingEnemy
    {
        void MoveTo(Vector2 destination);
    }
}
