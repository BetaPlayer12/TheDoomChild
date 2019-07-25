

namespace DChild.Gameplay.Characters.Players.State
{
    public interface IPlayerAnimationState
    {
        bool isFromFall { get; set; }
        bool isAnticPlayed { get; set; }
        bool hasDoubleJumped { get; set; }
        bool hasAttacked { get; set; }
        bool isLanding { get; set; }
        bool isFallingToJog { get; set; }
        bool isFromJog { get; set; }
        bool hasJumped { get; set; }
        bool isShortJumping { get; set; }
        bool isFromIdle { get; set; }
        bool isStaticJumping { get; set; }
        bool hasDashed { get; set; }
        bool hasWallSticked { get; set; }
        bool isWallJumping { get; set; }
        bool onWallJumpEnd { get; set; }
        bool isThrowingBomb { get; set; }
        bool transitionToFall2 { get; set; }
        bool isHardLanding { get; set; }
        bool isFallingFromWallJump { get; set; }
        bool isLedging { get ; set; }
        void ResetAnimations();
    }
}
