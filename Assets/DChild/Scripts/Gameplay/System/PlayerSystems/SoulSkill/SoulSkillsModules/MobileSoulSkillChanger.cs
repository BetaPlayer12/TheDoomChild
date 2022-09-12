#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public struct MobileSoulSkillChanger : ISoulSkillModule
    {
        public void AttachTo(int soulSkillInstanceID, IPlayer player)
        {
            GameplaySystem.soulSkillManager.ForceAllowSoulSkillActivation(true);
        }

        public void DetachFrom(int soulSkillInstanceID, IPlayer player)
        {
            GameplaySystem.soulSkillManager.ForceAllowSoulSkillActivation(false);
        }
    }
}