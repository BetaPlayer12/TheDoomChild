using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public interface IDamageReaction
    {
        void ReactToBeingAttackedBy(GameObject attacker, bool hasBlockedDamage);
    }
}