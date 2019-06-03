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
        private TrajectoryUI m_trajectoryUI;

        public TrajectorySimulator GetTrajectorySimulator() => m_trajectorySimulator;

        public void HideSimulation(TrajectorySimulator simulator)
        {
            m_trajectoryUI.Hide();
        }

        public void ShowSimulation(TrajectorySimulator simulator)
        {
            m_trajectoryUI.AttachTo(simulator);
            simulator.SimulateTrajectory();
            m_trajectoryUI.Show();
        }
    }
}