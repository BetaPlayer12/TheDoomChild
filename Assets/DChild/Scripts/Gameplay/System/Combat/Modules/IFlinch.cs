using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public interface IFlinch
    {
        void Flinch(Vector2 directionToSource, RelativeDirection damageSource, AttackSummaryInfo attackInfo);
    }
}