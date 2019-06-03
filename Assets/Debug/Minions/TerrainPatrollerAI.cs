using DChild.Gameplay.Characters.AI;
using UnityEngine;

namespace DChildDebug.Gameplay.Characters.AI
{
    public class TerrainPatrollerAI : MonoBehaviour
    {
        private ITerrainPatroller m_patroler;
        private TerrainPatrolSensor m_sensor;

        private void Start()
        {
            m_patroler = GetComponent<ITerrainPatroller>();
            m_sensor = GetComponentInChildren<TerrainPatrolSensor>();
        }

        private void Update()
        {
            if (m_patroler.waitForBehaviourEnd)
                return;

            if (m_sensor.shouldTurnAround)
            {
                m_patroler.Turn();
            }
            else
            {
                m_patroler.Move();
            }
        }
    }
}