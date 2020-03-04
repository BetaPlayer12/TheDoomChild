using UnityEngine;
using Sirenix.OdinInspector;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pathfinding;
using Holysoft.Collections;
using DChild.Gameplay.Characters.AI;

namespace DChild.Gameplay.Characters.Enemies
{
    public class WhiteLadyBrain : FlyingMinionAIBrain<WhiteLady>, IAITargetingBrain
    {
        [SerializeField]
        [BoxGroup("Specter Summon")]
        private GameObject[] m_summonSpecters;

        [SerializeField]
        [BoxGroup("Specter Summon")]
        private Holysoft.Collections.RangeInt m_numberOfSummons;
        [SerializeField]
        [BoxGroup("Specter Summon")]
        private RangeFloat m_summonRange;

        private bool m_hasSummonedSpecters;

        private WayPointPatroler m_patrol;

        public override void Enable(bool value)
        {
            enabled = value;
        }

        public override void ResetBrain()
        {
            m_minion.Idle();
            m_hasSummonedSpecters = false;
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
                m_minion.MoveTo(destination);
            }
            else
            {
                m_minion.Turn();
            }
        }

        private WhiteLady.SpecterSummonInfo[] GenerateSummonInfo()
        {
            var summonCount = m_numberOfSummons.GenerateRandomValue();
            WhiteLady.SpecterSummonInfo[] summonInfo = new WhiteLady.SpecterSummonInfo[summonCount];
            for (int i = 0; i < summonCount; i++)
            {
                var specterIndex = Random.Range(0, m_summonSpecters.Length);
                var specter = m_summonSpecters[specterIndex];
                var summonXPosOffset = m_summonRange.GenerateRandomValue() * Mathf.Sign(Random.Range(-1f, 1f));
                var summonYPosOffset = m_summonRange.GenerateRandomValue() * Mathf.Sign(Random.Range(-1f, 1f));
                var summonPosition = m_minion.position + new Vector2(summonXPosOffset, summonYPosOffset);
                summonInfo[i] = new WhiteLady.SpecterSummonInfo(specter, summonPosition);
            }
            return summonInfo;         
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrol = GetComponent<WayPointPatroler>();
        }

        public void Update()
        {
            if (m_minion.waitForBehaviourEnd)
                return;

            if (m_hasSummonedSpecters)
            {
                
            }
            else
            {
                if (m_target == null)
                {
                    Patrol();
                }
                else
                {
                    var targetPos = m_target.position;

                    if (IsLookingAt(targetPos))
                    {
                        m_minion.SummonSpecters(GenerateSummonInfo(), m_target);
                    }
                    else
                    {
                        m_minion.Turn();
                    }
                }
            }
        }
    }
}