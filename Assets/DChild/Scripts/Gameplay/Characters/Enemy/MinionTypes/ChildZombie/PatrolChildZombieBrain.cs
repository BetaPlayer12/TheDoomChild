using DChild.Gameplay.Characters.AI;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    [RequireComponent(typeof(WayPointPatroler))]
    public class PatrolChildZombieBrain : ChildZombieBrain
    {
        private WayPointPatroler m_patrol;

        public override void Enable(bool value)
        {
            m_target = null;
            m_minion.Idle();
        }

        protected override void Idle()
        {
            var patrolInfo = m_patrol.GetInfo(m_minion.position);
            m_minion.PatrolTo(patrolInfo.destination);
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrol = GetComponent<WayPointPatroler>();
        }
    }

}