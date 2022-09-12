using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [System.Serializable]
    public struct CriticalDamageInfo
    {
        [Range(0, 100)]
        public int chance;
        [MinValue(0), ShowIf("@chance != 0")]
        public int damageModifier;

        public CriticalDamageInfo(int critChance, int critDamageModifier)
        {
            this.chance = critChance;
            this.damageModifier = critDamageModifier;
        }
    }
}
