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
        private int m_damageValue;

        public void AttachTo(int soulSkillInstanceID, IPlayer player)
        {
            player.weapon.SetAddedDamage(m_damageValue);
        }

        public void DetachFrom(int soulSkillInstanceID, IPlayer player)
        {
            player.weapon.SetAddedDamage(-m_damageValue);
        }
    }
}