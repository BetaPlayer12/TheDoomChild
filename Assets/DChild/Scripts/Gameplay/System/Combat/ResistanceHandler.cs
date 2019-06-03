using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public struct ResistanceHandler
    {
        public void CalculatateResistanceReduction(IAttackResistance targetResistance, ref DamageInfo info)
        {
            var resistanceFactor = targetResistance.GetResistance(info.damageType);
            var damage = Mathf.FloorToInt(info.damage - (info.damage * resistanceFactor));
            info.damage = damage;
            info.isHeal = damage < 0;
        }

        internal void CalculatateResistanceReduction(IAttackResistance attackResistance, ref object damageInfo)
        {
            throw new NotImplementedException();
        }
    }
}