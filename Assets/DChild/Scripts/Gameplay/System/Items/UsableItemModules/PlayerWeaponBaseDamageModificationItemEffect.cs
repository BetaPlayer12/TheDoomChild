using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusAilment;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    [System.Serializable]
    public struct PlayerWeaponBaseDamageModificationItemEffect : IDurationItemEffect
    {
        [SerializeField]
        private DamageType m_type;
        [SerializeField, SuffixLabel("%", overlay: true)]
        private int m_addeddamage;
        private int originaldamage;

        public void StartEffect(IPlayer player)
        {
            Damage temp = player.weapon.damage;
            temp.type = m_type;
            originaldamage = temp.value;
            float damage = 25 * (m_addeddamage / 100f);
            temp.value = (int)Math.Ceiling(damage);
            player.weapon.SetBaseDamage(temp);
        }

        public void StopEffect(IPlayer player)
        {
            Damage temp = player.weapon.damage;
            temp.type = DamageType.Physical;
            temp.value = originaldamage;
            player.weapon.SetBaseDamage(temp);
        }
    }
}
