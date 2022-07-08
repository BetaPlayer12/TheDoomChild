#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public struct WallCrawl : ISoulSkillModule
    {

        public void AttachTo(int soulSkillInstanceID, IPlayer player)
        {
            player.state.canWallCrawl = true;
        }

        public void DetachFrom(int soulSkillInstanceID, IPlayer player)
        {
            player.state.canWallCrawl = false;
        }
    }
}