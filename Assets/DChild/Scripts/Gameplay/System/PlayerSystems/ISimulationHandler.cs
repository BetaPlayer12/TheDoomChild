using DChild.Gameplay.Combat;

namespace DChild.Gameplay
{
    public interface ISimulationHandler
    {
        TrajectorySimulator GetTrajectorySimulator();
        void ShowSimulation(TrajectorySimulator simulator);
        void HideSimulation(TrajectorySimulator simulator);
    }

}