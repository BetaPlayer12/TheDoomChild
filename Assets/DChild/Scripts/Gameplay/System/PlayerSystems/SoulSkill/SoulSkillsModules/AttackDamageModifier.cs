using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using System;
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
            Damage temp = player.weapon.damage;
            float damage = temp.value * (m_damageValue / 100f);
            int Calculateddamage = (int)Math.Ceiling(damage);
            player.weapon.SetAddedDamage(Calculateddamage);
        }

        public void DetachFrom(int soulSkillInstanceID, IPlayer player)
        {
            player.weapon.SetAddedDamage(0);
        }
    }
}