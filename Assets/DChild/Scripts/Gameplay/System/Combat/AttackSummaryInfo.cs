using System.Collections.Generic;

namespace DChild.Gameplay.Combat
{
    public struct AttackSummaryInfo
    {
        public bool isCrit { get; set; }
        public bool wasBlocked { get; set; }
        public DamageInfo damageInfo { get; private set; }

        public int damageDealt => damageInfo.damage.value;

        public AttackSummaryInfo(Damage damage)
        {
            isCrit = false;
            wasBlocked = false;
            damageInfo = new DamageInfo(damage);
        }

        public void SetDamageInfo(Damage damage)
        {
            damageInfo = new DamageInfo(damage);
        }
    }
}