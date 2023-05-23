using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusAilment;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    [System.Serializable]
    public struct PlayerWeaponBaseDamageModificationItemEffect : IDurationItemEffect
    {
        [SerializeField]
        private DamageType m_type;
        

        public void StartEffect(IPlayer player)
        {
            Damage temp = player.weapon.damage;
            temp.type = m_type;
            player.weapon.SetBaseDamage(temp);
        }

        public void StopEffect(IPlayer player)
        {
            Damage temp = player.weapon.damage;
            temp.type = DamageType.Physical;
            player.weapon.SetBaseDamage(temp);
        }
    }
}
