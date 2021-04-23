using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerDamageable : Damageable
    {
        [SerializeField]
        private Health m_armor;

        public override void TakeDamage(int totalDamage, AttackType type)
        {
            if (m_armor?.isEmpty ?? true)
            {
                base.TakeDamage(totalDamage, type);
            }
            else
            {
                m_armor?.ReduceCurrentValue(totalDamage);
            }
            CallDamageTaken(totalDamage, type);
        }
    }
}
