using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusAilment;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    [System.Serializable]
    public struct PlayerWeaponStatusInflictorModifierItemEffect : IDurationItemEffect
    {
        [SerializeField]
        private StatusEffectType m_type;
        [SerializeField, SuffixLabel("%", overlay: true)]
        private int m_statusinflictchance;

        public void StartEffect(IPlayer player)
        {

            player.weapon.SetInfliction(m_type, m_statusinflictchance);
        }

        public void StopEffect(IPlayer player)
        {
            player.weapon.SetInfliction(m_type, 0);
        }
    }
}
