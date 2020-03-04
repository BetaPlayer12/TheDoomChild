using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class DeformedCultistBrain : MinionAIBrain<DeformedCultist>, IAITargetingBrain
    {
        [SerializeField]
        [MinValue(0f)]
        private float m_spellRange;

        private bool m_isSpellActive;
        private WayPointPatroler m_patrol;

        public override void Enable(bool value)
        {
            throw new System.NotImplementedException();
        }

        public override void ResetBrain()
        {
            m_isSpellActive = false;
        }

        public void SetTarget(IEnemyTarget target)
        {
            throw new System.NotImplementedException();
        }

        private void Patrol()
        {
            var destination = m_patrol.GetInfo(m_minion.position).destination;
            if (IsLookingAt(destination))
            {
                m_minion.MoveTo(destination);
            }
            else
            {
                m_minion.Turn();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrol = GetComponent<WayPointPatroler>();
        }

        private void Update()
        {
            if (m_minion.waitForBehaviourEnd)
                return;
            if(m_target == null)
            {
                Patrol();
            }
            else
            {
                if (m_isSpellActive)
                {
                    m_minion.Taunt();
                }
                else if(Vector2.Distance(m_minion.position,m_target.position) <= m_spellRange)
                {
                    if (IsLookingAt(m_target.position))
                    {
                       m_minion.CastSpellAt(m_target);
                    }
                    else
                    {
                        m_minion.Turn();
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