using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [RequireComponent(typeof(LineRenderer))]
    public class TrajectoryLine : TrajectoryUI
    {
        private LineRenderer m_lineRenderer;

        public override void Show()
        {
            m_lineRenderer.enabled = true;
        }

        public override void Hide()
        {
            m_lineRenderer.enabled = false;
        }

        protected override void SimulationEnd(object sender, TrajectorySimulator.SimulationResultEventArgs eventArgs)
        {
            var positions = eventArgs.result;
            var positionCount = positions.Count;
            m_lineRenderer.positionCount = positionCount;
            for (int i = 0; i < positionCount; i++)
            {
                m_lineRenderer.SetPosition(i, positions[i]);
            }
        }

        protected override void Awake()
        {
            m_lineRenderer = GetComponent<LineRenderer>();
            base.Awake();
        }    
    }
}