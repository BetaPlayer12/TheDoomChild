namespace DChild.Gameplay.Characters.Players.State
{
    public interface IAttackState
    {
        bool isDoingCombo { get; set; }
        bool isChargingAttack { get; set; }
        bool canAttack { get; set; }
        bool isAttacking { get; set; }
        bool waitForBehaviour { get; set; }
        bool isDoingSwordThrust { get; set; }
    }
}