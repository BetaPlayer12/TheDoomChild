using System.Collections.Generic;

namespace DChild.Gameplay.Combat
{
    public struct AttackInfo
    {
        public bool isCrit { get; set; }
        public List<DamageInfo> damageList { get; }
        public int totalDamageDealt { get; private set; }

        public AttackInfo(params AttackDamage[] attackDamage)
        {
            isCrit = false;
            damageList = new List<DamageInfo>();
            totalDamageDealt = 0;
            for (int i = 0; i < attackDamage.Length; i++)
            {
                damageList.Add(new DamageInfo(attackDamage[i]));
                totalDamageDealt += attackDamage[i].value;
            }
        }

        public void Calculate()
        {
            totalDamageDealt = 0;
            for (int i = 0; i < damageList.Count; i++)
            {
                if (damageList[i].isHeal == false)
                {
                    totalDamageDealt += damageList[i].damage.value;
                }
            }
        }
    }
}