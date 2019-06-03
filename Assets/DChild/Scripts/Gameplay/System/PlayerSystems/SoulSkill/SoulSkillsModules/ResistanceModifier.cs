using DChild.Gameplay.Combat;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public struct ResistanceModifier : ISoulSkillModule
    {
        [SerializeField]
        private AttackType m_type;
        [SerializeField]
        private float m_addedResistance;

        public void AttachTo(IPlayer player)
        {
            player.attackResistance.AddResistance(m_type, m_addedResistance);
        }

        public void DetachFrom(IPlayer player)
        {
            player.attackResistance.ReduceResistance(m_type, m_addedResistance);
        }
    }
}