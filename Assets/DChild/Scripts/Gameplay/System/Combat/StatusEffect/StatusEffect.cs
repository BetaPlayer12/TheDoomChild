using DChild.Gameplay.Pooling;
using Holysoft.Collections;
using Holysoft.Event;
using System;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusInfliction
{
    public abstract class StatusEffect : PoolableObject, IStatusEffectEvents
    {
        [SerializeField]
        private CountdownTimer m_duration;
        private IStatusReciever m_reciever;

        public abstract StatusEffectType type { get; }
        public event EventAction<StatusEffectEventArgs> EffectStart;
        public event EventAction<StatusEffectEventArgs> EffectEnd;

        public virtual void StopEffect()
        {
            enabled = false;
            m_reciever.statusEffectState?.ChangeStatus(type, false);         
            EffectEnd?.Invoke(this, new StatusEffectEventArgs(m_reciever, this) );
        }

        public virtual void StartEffect()
        {
            enabled = true;
            m_duration.Reset();
            m_reciever.statusEffectState?.ChangeStatus(type, true);
            EffectStart?.Invoke(this, new StatusEffectEventArgs(m_reciever, this));
        }

        public void SetDuration(float duration) => m_duration.SetStartTime(duration);

        public virtual void SetReciever(IStatusReciever reciever) => m_reciever = reciever;
        public virtual void UpdateEffect(float deltaTime)
        {
            m_duration.Tick(GameplaySystem.time.deltaTime);
        }

        private void OnDurationEnd(object sender, EventActionArgs eventArgs)
        {
            StopEffect();
        }

        protected virtual void Awake()
        {

            m_duration.CountdownEnd += OnDurationEnd;
#if UNITY_EDITOR
            m_reciever = GetComponentInParent<IStatusReciever>();
            if (m_reciever != null)
            {
                SetReciever(m_reciever);
            }
#endif
        }

#if UNITY_EDITOR
        public static string[] GetStatusEffectTypes()
        {
            return DChildUtility.GetDerivedClassNames<StatusEffect>();
        }
#endif
    }
}