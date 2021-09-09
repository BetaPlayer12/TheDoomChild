using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public struct ResistanceHandler
    {
        public void CalculatateResistanceReduction(IAttackResistance targetResistance, AttackInfo info)
        {
            for (int i = 0; i < info.damageList.Count; i++)
            {
                var damageInfo = info.damageList[i];
                var damage = damageInfo.damage;
                var resistanceFactor = targetResistance.GetResistance(damage.type);
                damage.value = Mathf.FloorToInt(damage.value - (damage.value * resistanceFactor));
                damageInfo.damage = damage;
                damageInfo.isHeal = damage.value < 0;
                info.damageList[i] = damageInfo;
            }
        }
    }
}