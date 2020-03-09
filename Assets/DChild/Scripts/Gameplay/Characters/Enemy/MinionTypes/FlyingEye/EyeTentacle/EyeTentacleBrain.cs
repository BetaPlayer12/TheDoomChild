using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;

namespace DChild.Gameplay.Characters.Enemies
{
    public class EyeTentacleBrain : FlyingEyeBrain<EyeTentacle>, IAITargetingBrain
    {
        public override void Enable(bool value)
        {
            enabled = value;
        }

        public override void ResetBrain()
        {
            m_target = null;
            m_minion.RetractSpikes();
        }

        public void SetTarget(IEnemyTarget target)
        {
            m_target = target;
        }

        private void Update()
        {
            if (m_minion.waitForBehaviourEnd)
                return;
            Patrol();
            if (m_target == null)
            {
                m_minion.RetractSpikes();
            }
            else
            {
                m_minion.ExtendSpikes();
            }
        }
    }

}