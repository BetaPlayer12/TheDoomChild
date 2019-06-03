namespace DChild.Gameplay.Combat.StatusInfliction
{
    public class Stun : ImmobolizingStatusEffect
    {
        public override StatusEffectType type => StatusEffectType.Stun;

        public override void StartEffect()
        {
            base.StartEffect();
            m_character.DisableController();
        }

        public override void StopEffect()
        {
            base.StopEffect();
            m_character.EnableController();
        }
    }
}