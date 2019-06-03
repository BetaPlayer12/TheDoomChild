using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public abstract class TrajectoryUI : MonoBehaviour
    {
        [SerializeField]
        private TrajectorySimulator m_simulator;

        public abstract void Show();
        public abstract void Hide();

        public void AttachTo(TrajectorySimulator simulator)
        {
            if (m_simulator != simulator)
            {
                if (m_simulator != null)
                {
                    m_simulator.SimulationEnd -= SimulationEnd;
                }
                m_simulator = simulator;
                if (m_simulator != null)
                {
                    m_simulator.SimulationEnd += SimulationEnd;
                }
            }
        }

        protected abstract void SimulationEnd(object sender, TrajectorySimulator.SimulationResultEventArgs eventArgs);

        protected virtual void Awake()
        {
            if (m_simulator != null)
            {
                m_simulator.SimulationEnd += SimulationEnd;
            }
        }
    }
}