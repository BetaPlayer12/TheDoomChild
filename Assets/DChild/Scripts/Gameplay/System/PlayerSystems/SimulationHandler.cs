using DChild.Gameplay.Combat;
using DChild.Gameplay.Systems;
using UnityEngine;

namespace DChild.Gameplay
{
    public class SimulationHandler : MonoBehaviour, ISimulationHandler, IGameplaySystemModule
    {
        [SerializeField]
        private TrajectorySimulator m_trajectorySimulator;
        [SerializeField]
        private TrajectoryUI[] m_trajectoryUIs;

        public TrajectorySimulator GetTrajectorySimulator() => m_trajectorySimulator;

        public void HideSimulation(TrajectorySimulator simulator)
        {
            for (int i = 0; i < m_trajectoryUIs.Length; i++)
            {
                m_trajectoryUIs[i].Hide();
            }
        }

        public void ShowSimulation(TrajectorySimulator simulator)
        {
            for (int i = 0; i < m_trajectoryUIs.Length; i++)
            {
                m_trajectoryUIs[i].AttachTo(simulator);
                m_trajectoryUIs[i].Show();
            }
            simulator.SimulateTrajectory();
        }
    }
}