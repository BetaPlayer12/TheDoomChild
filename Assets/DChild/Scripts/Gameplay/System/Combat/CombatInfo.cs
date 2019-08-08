using System.Collections.Generic;

namespace DChild.Gameplay.Combat
{
    public class CombatInfo : ICriticalDamageInfo
    {
        public CombatInfo()
        {
            attackerDamageType = new List<AttackType>();
            damageDealt = new List<AttackDamage>();
            physicalDamage = 0;
            magicDamage = 0;
            totalDamage = 0;
            heals = 0;
            isCrit = false;
        }

        public int physicalDamage { get; set; }
        public int magicDamage { get; set; }
        public int totalDamage { get; set; }
        public List<AttackType> attackerDamageType { get; }
        public List<AttackDamage> damageDealt { get; }
        public int heals { get; set; }
        public bool isCrit { get; set; }

        public void RecordDamageDealt(params AttackDamage[] damage)
        {
            Reset();
            damageDealt.AddRange(damage);
            for (int i = 0; i < damage.Length; i++)
            {
                var currentDamage = damage[i];
                if (AttackDamage.IsMagicAttack(currentDamage.type))
                {
                    magicDamage += currentDamage.value;
                }
                else
                {
                    physicalDamage += currentDamage.value;
                }
                totalDamage += currentDamage.value;
                attackerDamageType.Add(currentDamage.type);
                //damageDealt.Add(new AttackDamage(currentDamage.type, currentDamage.damage));
            }
        }

        /// <summary>
        /// Needs To Call Reset before recording Multiple damage for best results
        /// </summary>
        /// <param name="damageType"></param>
        /// <param name="damageValue"></param>
        public void RecordDamage(AttackType damageType, int damageValue)
        {
            if (AttackDamage.IsMagicAttack(damageType))
            {
                magicDamage += damageValue;
            }
            else
            {
                physicalDamage += damageValue;
            }
            totalDamage += damageValue;
            attackerDamageType.Add(damageType);
            damageDealt.Add(new AttackDamage(damageType, damageValue));
        }

        public void Reset()
        {
            attackerDamageType.Clear();
            damageDealt.Clear();
            physicalDamage = 0;
            magicDamage = 0;
            totalDamage = 0;
            heals = 0;
            isCrit = false;
        }
    }
}