using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public struct AttackDamageModifier : ISoulSkillModule
    {
        [SerializeField, HideLabel]
        private AttackDamage m_damage;

        public void AttachTo(int soulSkillInstanceID, IPlayer player)
        {
            player.weapon.SetAddedDamage(m_damage);
        }

        public void DetachFrom(int soulSkillInstanceID, IPlayer player)
        {
            var negativeDamage = m_damage;
            negativeDamage.value *= -1;
            player.weapon.SetAddedDamage(negativeDamage);
        }
    }
}