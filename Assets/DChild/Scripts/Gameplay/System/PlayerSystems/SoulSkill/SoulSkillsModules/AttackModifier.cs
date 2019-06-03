using DChild.Gameplay.Combat;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public struct AttackModifier : ISoulSkillModule
    {
        [SerializeField]
        private AttackType m_type;
        [SerializeField]
        private int m_addedDamage;

        public void AttachTo(IPlayer player)
        {
            player.equipment.weapon.AddDamage(m_type, m_addedDamage);
        }

        public void DetachFrom(IPlayer player)
        {
            player.equipment.weapon.ReduceDamage(m_type, m_addedDamage);
        }
    }
}