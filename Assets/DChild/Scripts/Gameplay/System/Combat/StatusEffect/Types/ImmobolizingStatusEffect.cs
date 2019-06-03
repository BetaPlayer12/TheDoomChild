using DChild.Gameplay.Characters;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusInfliction
{
    public abstract class ImmobolizingStatusEffect : StatusEffect
    {
        private Rigidbody2D m_rigidbody;
        protected CombatCharacter m_character;

        public override void StartEffect()
        {
            base.StartEffect();
            m_rigidbody.velocity = Vector2.zero;
        }

        public override void StopEffect()
        {
            base.StopEffect();
            m_rigidbody.velocity = Vector2.zero;
        }

        public override void SetReciever(IStatusReciever reciever)
        {
            base.SetReciever(reciever);
            m_rigidbody = reciever.GetComponentInChildren<Rigidbody2D>();
            m_character = reciever.GetComponentInChildren<CombatCharacter>();
        }
    }
}