using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Pathfinding;
using UnityEngine;

namespace DChildDebug.Gameplay.Characters.Enemies
{
    public class HoodlessSeekerBrain : FlyingMinionAIBrain<HoodlessSeeker>, IAITargetingBrain
    {
        private Vector3 m_idlePosition;
      
        private WayPointPatroler m_patrol;

        public override void Enable(bool value)
        {
            m_idlePosition = transform.position;
        }

        public override void ResetBrain()
        {
            m_idlePosition = transform.position;
        }

        public void SetTarget(IEnemyTarget target)
        {
            m_target = target;
            if(m_target == null)
            {
                Patrol();
                //m_minion.SafePhase();
            }
            else
            {
                m_minion.AlertPhase();
            }
        }

        private void Patrol()
        {
            var destination = m_patrol.GetInfo(m_minion.transform.position).destination;
            m_minion.PatrolTo(destination);
        }

        private void Start()
        {
            ResetBrain();
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrol = GetComponent<WayPointPatroler>();
        }

        private void Update()
        {
            if (m_target == null)
            {
                Patrol();
            }
            else
            {
                MoveTo(m_target.position);
            }
        }
    }

}
