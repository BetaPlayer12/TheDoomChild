using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public interface IBlock
    {
        void BlockAttack(GameObject attacker);
    }
}