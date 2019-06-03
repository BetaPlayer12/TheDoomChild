using DChild.Gameplay.Characters.AI;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class BookWormBrain : MinionAIBrain<BookWorm>, IAITargetingBrain
    {
        [SerializeField]
        [MinValue(0f)]
        private float m_acidSpitRange;
        [SerializeField]
        [MinValue(0f)]
        private float m_devourHeadRange;
        [SerializeField]
        [Range(0, 100)]
        private float m_chanceToDevourHead;
        [SerializeField]
        private CountdownTimer m_abilityCooldown;
        [SerializeField]
        private CountdownTimer m_devourHeadDamageInterval;
        [SerializeField]
        private CountdownTimer m_devourHeadDuration;

        private bool m_isAttacking;
        private bool m_abilityOnCooldown;

        private WayPointPatroler m_patrol;

        public override void Enable(bool value)
        {
            enabled = value;
        }

        public override void ResetBrain()
        {
            m_isAttacking = false;
            m_abilityOnCooldown = false;
            m_target = null;
        }

        public void SetTarget(IEnemyTarget target)
        {
            m_target = target;
        }

        private void Patrol()
        {
            var destination = m_patrol.GetInfo(transform.position).destination;
            if (IsLookingAt(destination))
            {
                m_minion.PatrolTo(destination);
            }
            else
            {
                m_minion.Turn();
            }
        }

        private void OnAbilityCooldownEnd(object sender, EventActionArgs eventArgs)
        {
            m_abilityOnCooldown = false;
        }

        private void OnDevourHeadDurationEnd(object sender, EventActionArgs eventArgs)
        {
            m_minion.LetGoOfHead();
            m_devourHeadDamageInterval.EndTime(true);
        }

        private void OnDamageIntervalEnd(object sender, EventActionArgs eventArgs)
        {
            //m_minion.DealDPSTo(m_target);
            m_devourHeadDamageInterval.Reset();
        }

        protected override void Awake()
        {
            base.Awake();
            m_abilityCooldown.CountdownEnd += OnAbilityCooldownEnd;
            m_devourHeadDamageInterval.CountdownEnd += OnDamageIntervalEnd;
            m_devourHeadDuration.CountdownEnd += OnDevourHeadDurationEnd;
        }

        private void Update()
        {
            if (m_minion.waitForBehaviourEnd)
                return;

            if (m_isAttacking)
            {
                m_isAttacking = false;
                m_abilityCooldown.Reset();
                m_abilityOnCooldown = true;
            }
            else if (m_minion.isAttachedToHead)
            {
                var deltaTime = Time.deltaTime;
                m_devourHeadDamageInterval.Tick(deltaTime);
                m_devourHeadDuration.Tick(deltaTime);
            }
            else if (m_abilityOnCooldown || m_target == null)
            {
                m_abilityCooldown.Tick(Time.deltaTime);
                Patrol();
            }
            else if (m_target != null)
            {
                if (IsLookingAt(m_target.position))
                {
                    var distanceToTarget = Vector2.Distance(m_minion.position, m_target.position);
                    if (distanceToTarget <= m_devourHeadRange)
                    {
                        var willDevourHeadOfTarget = UnityEngine.Random.Range(0, 100) <= m_chanceToDevourHead;
                        if (willDevourHeadOfTarget)
                        {
                            m_minion.DevourHead(m_target);
                        }
                        else
                        {
                            //Make it Smart
                            m_minion.SpitAcid(m_target);
                        }
                    }
                    else if (distanceToTarget <= m_acidSpitRange)
                    {
                        m_minion.SpitAcid(m_target);
                    }
                    else
                    {
                        Patrol();
                    }
                }
                else
                {
                    Patrol();
                }
            }
        }
    }

}