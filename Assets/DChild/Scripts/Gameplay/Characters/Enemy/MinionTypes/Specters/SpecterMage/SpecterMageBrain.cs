using DChild.Gameplay.Characters.AI;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class SpecterMageBrain : SpecterBrain<SpecterMage>, IAITargetingBrain
    {
        [SerializeField]
        [MinValue(0f)]
        private float m_spellCastDistance;
        [SerializeField]
        private CountdownTimer m_spellCooldown;
        private bool m_canCastSpell;
        private bool m_isCastingSpell;

        public override void Enable(bool value)
        {
            enabled = true;
        }

        public override void ResetBrain()
        {
            m_target = null;
            m_spellCooldown.EndTime(false);
            m_canCastSpell = true;
            m_isCastingSpell = false;
        }

        public void SetTarget(IEnemyTarget target)
        {
            m_target = target;
        }

        protected override void MoveAttackTarget()
        {
            var targetPosition = m_target.position;
            if (IsLookingAt(targetPosition))
            {
                if (Vector2.Distance(m_minion.position, m_target.position) <= m_spellCastDistance)
                {
                    if (m_canCastSpell)
                    {
                        m_minion.ConjurePlasma(m_target);
                        m_isCastingSpell = true;
                    }
                    else
                    {
                        m_minion.Idle();
                    }
                }
                else
                {
                    MoveTo(m_target.position);
                }
            }
            else
            {
                m_minion.Turn();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_spellCooldown.CountdownEnd += OnSpellCooldownEnd;
        }

        private void OnSpellCooldownEnd(object sender, EventActionArgs eventArgs) => m_canCastSpell = true;

        protected override void Update()
        {
            if (m_isCastingSpell && m_minion.waitForBehaviourEnd == false)
            {
                m_isCastingSpell = false;
                m_spellCooldown.Reset();
                m_canCastSpell = false;
            }
            else if (m_canCastSpell == false)
            {
                m_spellCooldown.Tick(m_minion.time.deltaTime);
            }

            base.Update();
        }
    }

}