using DChild.Gameplay.Combat.StatusInfliction;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public struct StatusEffectInfliction : ISoulSkillModule
    {
        [SerializeField]
        private StatusEffectType m_type;
        [SerializeField]
        private float m_chance;

        public void AttachTo(IPlayer player)
        {
            player.equipment.weapon.AddStatusInfliction(m_type, m_chance);
        }

        public void DetachFrom(IPlayer player)
        {
            player.equipment.weapon.ReduceStatusInfliction(m_type, m_chance);
        }
    }
}