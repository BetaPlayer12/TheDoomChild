using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public struct SkillModifier : ISoulSkillModule
    {
        [SerializeField]
        private PlayerModifier m_toChange;
        [SerializeField]
        private float m_value;

        public void AttachTo(int soulSkillInstanceID, IPlayer player)
        {
            ChangeSkillValue(player.modifiers, m_value);
            
        }

        public void DetachFrom(int soulSkillInstanceID, IPlayer player)
        {
            ChangeSkillValue(player.modifiers, 1);
        }

        private void ChangeSkillValue(PlayerModifierHandle modifier, float value)
        {
            modifier.Set(m_toChange, value);
        }
    }
}