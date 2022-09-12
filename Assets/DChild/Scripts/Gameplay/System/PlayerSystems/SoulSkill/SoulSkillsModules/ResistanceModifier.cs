using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public struct ResistanceModifier : ISoulSkillModule
    {
        [SerializeField]
        private DamageType m_type;
        [SerializeField, SuffixLabel("%", overlay: true)]
        private int m_addedResistance;

        public void AttachTo(int soulSkillInstanceID, IPlayer player)
        {
            player.attackResistance.AddResistance(m_type, m_addedResistance/100f);
        }

        public void DetachFrom(int soulSkillInstanceID, IPlayer player)
        {
            player.attackResistance.ReduceResistance(m_type, m_addedResistance/100f);
        }
    }
}