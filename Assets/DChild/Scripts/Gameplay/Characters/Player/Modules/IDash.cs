using DChild.Gameplay.Characters.Players.Behaviour;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IDash : IResettableBehaviour, ICancellableBehaviour
    {
        void HandleCooldown();
        void ResetCooldownTimer();
        void HandleDurationTimer();
        bool IsDashDurationOver();
        void ResetDurationTimer();

        void Execute();
    }
}
