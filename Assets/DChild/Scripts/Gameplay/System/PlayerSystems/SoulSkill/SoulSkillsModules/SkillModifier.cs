using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public struct SkillModifier : ISoulSkillModule
    {
        private enum SkillValue
        {
            Dash_Distance,
            Dash_Cooldown,
            WallStick_Duration,
            ShadowMorph_MagicRequirement,
        }

        [SerializeField]
        private SkillValue m_toChange;
        [SerializeField]
        private float m_value;

        public void AttachTo(IPlayer player)
        {
            ChangeSkillValue(player, m_value);
        }

        public void DetachFrom(IPlayer player)
        {
            ChangeSkillValue(player, 1);
        }

        private void ChangeSkillValue(IPlayer player, float value)
        {
            switch (m_toChange)
            {
                case SkillValue.Dash_Distance:
                    player.modifiers.dashDistance = value;
                    break;
                case SkillValue.Dash_Cooldown:
                    player.modifiers.dashCooldown = value;
                    break;
                case SkillValue.WallStick_Duration:
                    player.modifiers.stickDuration = value;
                    break;
                case SkillValue.ShadowMorph_MagicRequirement:
                    player.modifiers.shadowMagicRequirement = value;
                    break;
            }
        }
    }
}