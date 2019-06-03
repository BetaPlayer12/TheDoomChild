using Holysoft;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace DChild.Gameplay.Combat.StatusInfliction
{
    public class StatusEffectTracker
    {
        [OdinSerialize]
        private Dictionary<IStatusReciever, Dictionary<StatusEffectType, StatusEffect>> m_recieverTracker;

        public StatusEffectTracker()
        {
            m_recieverTracker = new Dictionary<IStatusReciever, Dictionary<StatusEffectType, StatusEffect>>();
        }

        public StatusEffect GetStatusEffectOf(IStatusReciever statusReciever, StatusEffectType type)
        {
            if (m_recieverTracker.ContainsKey(statusReciever))
            {
                var statusTracker = m_recieverTracker[statusReciever];
                return statusTracker.ContainsKey(type) ? statusTracker[type] : null;
            }
            else
            {
                return null;
            }
        }

        public void TrackEffect(IStatusReciever statusReciever, StatusEffectType type, StatusEffect instance)
        {
            if (m_recieverTracker.ContainsKey(statusReciever))
            {
                var statusTracker = m_recieverTracker[statusReciever];
                if (statusTracker.ContainsKey(type))
                {
                    statusTracker[type] = instance;
                }
                else
                {
                    m_recieverTracker[statusReciever].Add(type, instance);
                }
            }
            else
            {
                m_recieverTracker.Add(statusReciever, new Dictionary<StatusEffectType, StatusEffect>());
                m_recieverTracker[statusReciever].Add(type, instance);
                statusReciever.ReceiverDestroyed += OnRecieverDestroy;
            }

            instance.EffectEnd += OnEffectEnd;
        }

        private void OnEffectEnd(object sender, StatusEffectEventArgs eventArgs)
        {
            var statusTracker = m_recieverTracker[eventArgs.reciever];
            eventArgs.effect.EffectEnd -= OnEffectEnd;
            statusTracker.Remove(eventArgs.type);
        }

        private void OnRecieverDestroy(object sender, StatusRecieverEventArgs eventArgs)
        {
            var statusTracker = m_recieverTracker[eventArgs.reciever];
            foreach (var effect in statusTracker.Values)
            {
                effect.EffectEnd -= OnEffectEnd;
                effect.StopEffect();
            }
        }
    }
}