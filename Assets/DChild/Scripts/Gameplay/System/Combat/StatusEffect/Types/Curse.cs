namespace DChild.Gameplay.Combat.StatusInfliction
{
    public class Curse : StatusEffect
    {
        private Health m_maxHealth;

        public override StatusEffectType type => StatusEffectType.Curse;

        public override void StartEffect()
        {
            m_maxHealth.SetMaxValue(m_maxHealth.maxValue / 2);
            base.StartEffect();
        }

        public override void StopEffect()
        {
            base.StopEffect();
            m_maxHealth.SetMaxValue(m_maxHealth.maxValue * 2);
        }

        public override void SetReciever(IStatusReciever reciever)
        {
            base.SetReciever(reciever);
            m_maxHealth = reciever.GetComponentInChildren<Health>();
        }
    }
}