#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public interface ISoulSkillModule
    {
        void AttachTo(IPlayer player);
        void DetachFrom(IPlayer player);
    }
}