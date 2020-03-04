using DChild.Gameplay.Characters.AI;

namespace DChild.Gameplay.Characters.Enemies
{
    public class EyeSentryBrain : MinionAIBrain<EyeSentry>
    {
        //private TerrainPatrolSensor m_terrainSensor;

        public override void Enable(bool value)
        {       
        }

        public override void ResetBrain()
        {
        }

        private void Start()
        {
           // m_terrainSensor = GetComponentInChildren<TerrainPatrolSensor>();
        }

        private void Update()
        {
            if (m_minion.waitForBehaviourEnd)
                return;

            //if (m_terrainSensor.shouldTurnAround)
            //{
            //    m_minion.Turn();
            //}
            //else
            //{
            //    m_minion.Move();
            //}
        }
    }

}