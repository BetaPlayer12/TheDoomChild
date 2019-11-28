#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public interface ISoulSkillModule
    {
        void AttachTo(int soulSkillInstanceID, IPlayer player);
        void DetachFrom(int soulSkillInstanceID, IPlayer player);
    }
}