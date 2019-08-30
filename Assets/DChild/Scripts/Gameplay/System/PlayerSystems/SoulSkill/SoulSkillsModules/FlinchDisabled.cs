#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public struct FlinchDisabled : ISoulSkillModule
    {
        public void AttachTo(IPlayer player)
        {
            player.state.canFlinch = false;
        }

        public void DetachFrom(IPlayer player)
        {
            player.state.canFlinch = true;
        }
    }
}