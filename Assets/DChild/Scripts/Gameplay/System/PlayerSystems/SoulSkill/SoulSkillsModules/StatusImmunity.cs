#if UNITY_EDITOR
#endif

using DChild.Gameplay.Combat.StatusAilment;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public struct StatusImmunity : ISoulSkillModule
    {
        [SerializeField]
        private StatusEffectType m_type;

        public void AttachTo(IPlayer player)
        {
            player.statusResistance.SetResistance(m_type, 100);
        }

        public void DetachFrom(IPlayer player)
        {
            player.statusResistance.SetResistance(m_type, 0);
        }
    }
}