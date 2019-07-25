namespace DChild.Gameplay.Characters.Players.State
{
    public interface ICombatState
    {
        bool inCombat { get; set; }
        bool canAttack { get; set; }
        bool isAttacking { get; set; }
    }

    public interface IProjectileThrowState
    {
        bool canAttack { set; }
        bool waitForBehaviour { set; }
        bool isAimingProjectile { get; set; }
    }
}