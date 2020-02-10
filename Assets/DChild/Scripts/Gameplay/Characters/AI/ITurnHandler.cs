using UnityEngine;

namespace DChild.Gameplay.Characters.AI
{
    public interface ITurnHandler
    {
        void LookAt(Vector2 target);
    }
}
