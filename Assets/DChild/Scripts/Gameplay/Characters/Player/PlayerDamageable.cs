using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerDamageable : Damageable
    {
        [SerializeField]
        private Health m_ambrosia;

        public override void TakeDamage(int totalDamage, AttackType type)
        {
            if (m_ambrosia?.isEmpty ?? true)
            {
                base.TakeDamage(totalDamage, type);
            }
            else
            {
                m_ambrosia?.ReduceCurrentValue(totalDamage);
                CallDamageTaken(totalDamage, type);
            }
        }
    }
}
