//using DChild.Gameplay.Characters.AI;
//using DChild.Gameplay.Combat;
//using Sirenix.OdinInspector;
//using UnityEngine;

//namespace DChild.Gameplay.Characters.Enemies
//{
//    public class LichBrain : MinionAIBrain<Lich>, IAITargetingBrain
//    {
//        [SerializeField]
//        [MinValue(0f)]
//        private float m_ambushDistance;
//        [SerializeField]
//        [MinValue(0f)]
//        private float m_spellRange;

//        public override void Enable(bool value)
//        {
//            enabled = value;
//            if (m_minion.isHiding == false)
//            {
//                m_minion.Idle();
//            }
//        }

//        public override void ResetBrain()
//        {
//            if (m_minion.isHiding == false)
//            {
//                m_minion.HideSelf();
//            }
//        }

//        public void SetTarget(IEnemyTarget target)
//        {
//            m_target = target;
//        }

//        private void Update()
//        {
//            if (m_minion.waitForBehaviourEnd)
//                return;

//            if(m_target == null)
//            {
//                if(m_minion.isHiding == false)
//                {
//                    m_minion.HideSelf();
//                }
//            }
//            else
//            {
//                if (m_minion.isHiding && 
//                    Vector2.Distance(m_minion.position, m_target.position) <= m_ambushDistance)
//                {
//                    m_minion.ShowSelf();
//                }
//                else
//                {
//                    m_minion.CastSpell(m_target);
//                }
//            }
//        }
//    }

//}