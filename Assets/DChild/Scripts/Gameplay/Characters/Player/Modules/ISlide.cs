using DChild.Gameplay.Characters.Players.Behaviour;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface ISlide : IResettableBehaviour, ICancellableBehaviour
    {
        void HandleCooldown();
        void ResetCooldownTimer();
        void HandleDurationTimer();
        bool IsSlideDurationOver();
        void ResetDurationTimer();
        bool HasGroundToSlideOn();

        void Execute();
    }
}
