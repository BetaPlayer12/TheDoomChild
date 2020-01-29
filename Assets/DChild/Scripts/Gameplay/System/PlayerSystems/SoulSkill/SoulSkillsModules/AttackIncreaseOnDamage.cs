using Holysoft.Event;
using Refactor.DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public class AttackIncreaseOnDamage : HandledSoulSkillModule
    {
        private class Handle : BaseHandle
        {
            private int m_increaseValue;
            private int m_maxStacks;
            private WaitForWorldSeconds waitforSeconds;
            public int m_currentStacks;
            private Coroutine m_routine;

            public Handle(IPlayer m_player, int m_increaseValue, int m_maxStacks, float m_outOfCombatResetThreshold) : base(m_player)
            {

                this.m_increaseValue = m_increaseValue;
                this.m_maxStacks = m_maxStacks;
                waitforSeconds = new WaitForWorldSeconds(m_outOfCombatResetThreshold);
                this.m_currentStacks = 0;
            }

            public override void Initialize()
            {
                m_player.damageableModule.DamageTaken += OnDamaged;
                //m_player.CombatModeChanged += OnCombatModeChanged;
            }



            public override void Dispose()
            {
                m_player.damageableModule.DamageTaken -= OnDamaged;
                // m_player.CombatModeChanged -= OnCombatModeChanged;
            }

            private void OnCombatModeChanged(object sender, CombatStateEventArgs eventArgs)
            {
                if (eventArgs.inCombat)
                {
                    if (m_routine != null)
                    {
                        //m_player.StopCoroutine(m_routine);
                        m_routine = null;
                    }
                }
                else
                {
                    //m_routine = m_player.StartCoroutine(ResetRoutine());
                }
            }

            private void OnDamaged(object sender, Damageable.DamageEventArgs eventArgs)
            {
                if (m_currentStacks < m_maxStacks)
                {
                    m_currentStacks++;
                    //m_player.modifiers.damageModifier += m_increaseValue;
                }
            }


            private IEnumerator ResetRoutine()
            {
                yield return waitforSeconds;
                // m_player.modifiers.damageModifier -= m_increaseValue * m_currentStacks;
                m_currentStacks = 0;
            }
        }

        [SerializeField, MinValue(1)]
        private int m_increaseValue;
        [SerializeField, MinValue(1)]
        private int m_maxStack;
        [SerializeField, MinValue(0)]
        private float m_outOfCombatResetThreshold;

        protected override BaseHandle CreateHandle(IPlayer player) => new Handle(player, m_increaseValue, m_maxStack, m_outOfCombatResetThreshold);
    }
}