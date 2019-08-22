using DChild.Gameplay.Characters;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public interface IFlinch
    {
        void Flinch(Vector2 directionToSource, RelativeDirection damageSource, IReadOnlyCollection<AttackType> damageTypeRecieved);
    }
}