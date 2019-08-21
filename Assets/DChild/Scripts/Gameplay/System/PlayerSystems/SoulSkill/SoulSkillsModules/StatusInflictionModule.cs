using DChild.Gameplay.Combat.StatusAilment;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    [System.Serializable]
    public struct StatusInflictionModule : ISoulSkillModule
    {
        [SerializeField]
        private StatusEffectType m_type;
        [SerializeField, Range(0, 100)]
        private int m_chance;

        public void AttachTo(IPlayer player)
        {
            player.weapon.SetInfliction(m_type, m_chance);
        }

        public void DetachFrom(IPlayer player)
        {
            player.weapon.SetInfliction(m_type, m_chance);
        }
    }
}