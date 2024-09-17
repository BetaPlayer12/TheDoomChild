using Holysoft.Event;
using System;
using System.Collections.Generic;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyBattleOngoingEffectHandle
    {
        private class OngoingEffect
        {
            private Action m_effect;
            private int m_remainingRounds;

            public OngoingEffect(Action effect, int remainingRounds)
            {
                m_effect = effect;
                m_remainingRounds = remainingRounds;
            }

            public void ReduceRemainingRounds() => m_remainingRounds--;

            public bool IsOngoing() => m_remainingRounds > 0;

            public void UseEffect() => m_effect?.Invoke();
        }

        private List<OngoingEffect> m_ongoingEffects;

        public void RegisterOngoingAbility(Action effect, int round)
        {
            var ongoingEffect = new OngoingEffect(effect, round);
            ongoingEffect.UseEffect();
            m_ongoingEffects.Add(ongoingEffect);
        }

        public void Initialize(ArmyBattleHandle_TBD armyBattleHandle)
        {
            m_ongoingEffects = new List<OngoingEffect>();

            armyBattleHandle.RoundEnd += OnRoundeEnd;
            armyBattleHandle.RoundStart += OnRoundStart;
            armyBattleHandle.BattleEnd += OnBattleEnd;
        }

        private void OnBattleEnd(object sender, EventActionArgs eventArgs)
        {
            m_ongoingEffects.Clear();
        }

        private void OnRoundStart(object sender, EventActionArgs eventArgs)
        {
            for (int i = 0; i < m_ongoingEffects.Count; i++)
            {
                m_ongoingEffects[i].UseEffect();
            }
        }

        private void OnRoundeEnd(object sender, EventActionArgs eventArgs)
        {
            for (int i = m_ongoingEffects.Count - 1; i >= 0; i--)
            {
                var effect = m_ongoingEffects[i];
                effect.ReduceRemainingRounds();
                if (effect.IsOngoing() == false)
                {
                    m_ongoingEffects.RemoveAt(i);
                }
            }
        }
    }
}