namespace DChild.Gameplay.Characters.Players.State
{
    public interface ICombatState
    {
        bool inCombat { get; set; }
        bool canAttack { get; set; }
        bool waitForBehaviour { set; }
        bool isAttacking { get; set; }
        bool isGrounded { get; }
    }
}