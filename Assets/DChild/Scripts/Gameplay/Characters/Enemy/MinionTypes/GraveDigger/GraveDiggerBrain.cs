using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Environment;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class GraveDiggerBrain : MinionAIBrain<GraveDigger>, IAITargetingBrain
    {
        [SerializeField]
        [MinValue(0f)]
        private float m_attackRange;

        [SerializeField]
        private CountdownTimer m_attackRest;

        private bool m_isAttacking;
        private bool m_isResting;


        public override void Enable(bool value)
        {
            if (!value)
            {
                m_target = null;
            }
            else
            {
                m_minion.Stay();
            }
            enabled = value;
        }

        public override void ResetBrain()
        {
            m_minion.Stay();
            m_target = null;
            m_attackRest.EndTime(false);
            m_isAttacking = false;
            m_isResting = false;
        }

        public void SetTarget(IEnemyTarget target)
        {
            m_target = target;
        }

        private void OnAttackRestEnd(object sender, EventActionArgs eventArgs) => m_isResting = false;

        protected override void Awake()
        {
            base.Awake();
            m_attackRest.CountdownEnd += OnAttackRestEnd;
        }

        private void Update()
        {
            if (m_minion.waitForBehaviourEnd)
                return;

            if (m_isAttacking)
            {
                m_isResting = true;
                m_attackRest.Reset();
                m_isAttacking = false;
            }
            else if (m_isResting)
            {
                m_isResting = false;
                m_attackRest.Tick(m_minion.time.deltaTime);
                m_minion.Stay();
            }

            if (m_target == null)
            {
                m_minion.Stay();
            }
            else
            {
                var targetPos = m_target.position;

                if (IsLookingAt(targetPos))
                {
                    if (Vector2.Distance(m_minion.position, m_target.position) <= m_attackRange)
                    {
                        m_minion.ShovelAttack();
                        m_isAttacking = true;
                    }
                    else
                    {
                        m_minion.MoveTo(targetPos);
                    }
                }
                else
                {
                    m_minion.Turn();
                }
            }
        }
    }
}
