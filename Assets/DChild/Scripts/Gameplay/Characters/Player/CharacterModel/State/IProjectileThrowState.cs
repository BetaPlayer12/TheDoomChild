namespace DChild.Gameplay.Characters.Players.State
{
    public interface IProjectileThrowState
    {
        bool waitForBehaviour { set; }
        bool isAimingProjectile { get; set; }
    }
}