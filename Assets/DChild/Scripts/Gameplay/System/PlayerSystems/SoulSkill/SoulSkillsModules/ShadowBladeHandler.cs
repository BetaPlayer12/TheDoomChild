#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public struct ShadowBladeHandler : ISoulSkillModule
    {

        public void AttachTo(int soulSkillInstanceID, IPlayer player)
        {
            player.state.isShadowBlade = true;
        }

        public void DetachFrom(int soulSkillInstanceID, IPlayer player)
        {
            player.state.isShadowBlade = false;
        }
    }
}