#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public struct ShadowMorphCombat : ISoulSkillModule
    {

        public void AttachTo(int soulSkillInstanceID, IPlayer player)
        {
            player.state.canAttackInShadowMode = true;
        }

        public void DetachFrom(int soulSkillInstanceID, IPlayer player)
        {
            player.state.canAttackInShadowMode = false;
        }
    }
}