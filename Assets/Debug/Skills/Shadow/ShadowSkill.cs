namespace DChild.Gameplay.Characters.Players.Skill
{
    public abstract class ShadowSkill : MagicSkill
    {
        private IShadowSkillModifier m_shadowModifier;

        protected sealed override float CalculateMagicRequired() => base.CalculateMagicRequired() * m_shadowModifier.shadowMagicRequirement;

        public override void Initialize(IPlayerModules player)
        {
            base.Initialize(player);
            m_shadowModifier = player.modifiers;
        }
    }

}