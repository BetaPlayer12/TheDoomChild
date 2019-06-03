using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace DChild.Gameplay.Combat.StatusInfliction
{
    public class StatusEffectUpdateHandler
    {
        [ShowInInspector]
        private List<StatusEffect> m_list;

        public StatusEffectUpdateHandler()
        {
            m_list = new List<StatusEffect>();
        }

        public void Add(StatusEffect effect)
        {
            m_list.Add(effect);
            effect.EffectEnd += OnEffectEnd;
        }

        public void Update()
        {
            var deltaTime = GameplaySystem.time.deltaTime;
            for (int i = m_list.Count - 1; i >= 0; i--)
            {
                m_list[i].UpdateEffect(deltaTime);
            }
        }

        private void OnEffectEnd(object sender, StatusEffectEventArgs eventArgs)
        {
            eventArgs.effect.EffectEnd -= OnEffectEnd;
            m_list.Remove(eventArgs.effect);
        }
    }
}