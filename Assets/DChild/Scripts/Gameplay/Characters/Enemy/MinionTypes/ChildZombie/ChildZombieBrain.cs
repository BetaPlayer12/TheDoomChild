using DChild.Gameplay.Characters.AI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public abstract class ChildZombieBrain : MinionAIBrain<ChildZombie>, IAITargetingBrain
    {
        [SerializeField]
        [MinValue(0f)]
        private float m_projectileVomitDistance;
        [SerializeField]
        [MinValue(0f)]
        private float m_scratchDistance;
        [SerializeField]
        private float m_vomitChance;

#if UNITY_EDITOR
        public float projectileVomitDistance => m_projectileVomitDistance;
        public float scratchDistance => m_scratchDistance;
#endif

        protected abstract void Idle();

        public override void ResetBrain()
        {
            m_minion.Idle();
        }

        public void SetTarget(IEnemyTarget target)
        {
            m_target = target;
        }

        protected void Update()
        {
            if (m_minion.waitForBehaviourEnd)
                return;

            if (m_target == null)
            {
                Idle();
                var willVomit = UnityEngine.Random.Range(0f, 100f) <= m_vomitChance;
                if (willVomit)
                {
                    m_minion.Vomit();
                }
            }
            else
            {
                if (IsLookingAt(m_target.position))
                {
                    var distanceToTarget = Vector2.Distance(m_target.position, m_minion.position);
                    if (distanceToTarget > m_projectileVomitDistance)
                    {
                        m_minion.MoveTo(m_target.position);
                    }
                    else if (distanceToTarget <= m_scratchDistance)
                    {
                        m_minion.Scratch();
                    }
                    else
                    {
                        m_minion.ProjectileVomit(m_target.position);
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