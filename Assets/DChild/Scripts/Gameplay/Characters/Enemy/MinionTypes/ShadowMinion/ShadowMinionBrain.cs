using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class ShadowMinionBrain : MinionAIBrain<ShadowMinion>, IAITargetingBrain
    {
        [SerializeField]
        [MinValue(0f)]
        private float m_alertRange;
        private Vector3 m_chargeTo;

        public override void Enable(bool value)
        {

        }

        public override void ResetBrain()
        {
            m_target = null;
            m_minion.EnterShadowForm();
            m_chargeTo = Vector3.zero;
        }

        public void SetTarget(IEnemyTarget target)
        {
            if (m_target == null)
            {
                m_target = target;
            }
        }

        private void Update()
        {
            if (m_minion.waitForBehaviourEnd)
                return;

            if (m_target != null)
            {
                if (Vector2.Distance(m_minion.position, m_target.position) > m_alertRange)
                {
                    Debug.Log("Hide");
                    m_target = null;
                    m_minion.EnterShadowForm();
                }
                else if (m_minion.isCharging)
                {
                    if (Vector2.Distance(m_minion.position, m_chargeTo) <= 0.5f)
                    {
                        m_minion.StopCharge();
                    }
                }
                else
                {
                    Debug.Log("Do Whatever");
                    if (m_minion.inShadowForm)
                    {
                        m_minion.ExitShadowForm();
                    }
                    else
                    {
                        m_chargeTo = m_target.position;
                        m_chargeTo.y = transform.position.y;
                        m_minion.Charge(m_chargeTo);
                    }
                }
            }
        }
    }

}