namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IChargeAttackBehaviour
    {
        void StartCharge();
        void HandleCharge();

        bool IsChargeComplete();
        void Cancel();
        void Execute();
        void EndExecution();
    }
}
