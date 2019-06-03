using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusInfliction
{
    public abstract class DamagingStatusEffect : StatusEffect
    {
        [SerializeField]
        [PropertyOrder(1)]
        private CountdownTimer m_damageInterval;

        private IDamageable m_damageable;
        protected abstract AttackDamage damage { get; }

        public override void SetReciever(IStatusReciever reciever)
        {
            base.SetReciever(reciever);
            m_damageable = reciever.GetComponentInChildren<IDamageable>();
        }

        public override void StartEffect()
        {
            base.StartEffect();
            ApplyDamage();
        }

        public override void UpdateEffect(float deltaTime)
        {
            base.UpdateEffect(deltaTime);
            m_damageInterval.Tick(deltaTime);
        }

        private void ApplyDamage()
        {
            GameplaySystem.combatManager.Damage(m_damageable, damage);
            if (m_damageable.isAlive)
            {
                m_damageInterval.Reset();
            }
        }

        private void DamageIntervalEnd(object sender, EventActionArgs eventArgs)
        {
            ApplyDamage();
        }

        protected override void Awake()
        {
            base.Awake();
            m_damageInterval.CountdownEnd += DamageIntervalEnd;
        }
    }
}