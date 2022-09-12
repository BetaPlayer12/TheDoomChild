using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public struct DamageResistanceHandle
    {
        public Damage CalculatateResistedDamage(Damage damageRecieved, IAttackResistance targetResistance)
        {
            var resistanceFactor = targetResistance.GetResistance(damageRecieved.type);
            damageRecieved.value = Mathf.FloorToInt(damageRecieved.value - (damageRecieved.value * resistanceFactor));
            return damageRecieved;
        }
    }
}