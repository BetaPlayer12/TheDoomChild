namespace DChild.Gameplay.Characters.Enemies
{
    public class SkeletonWarriorAnimation : CombatCharacterAnimation
    {
        public const string ANIMATION_FLINCH = "Flinch";
        public const string ANIMATION_TURN = "Turn";
        public const string ANIMATION_DETECT = "Detects_Enemy";
        public const string ANIMATION_SLASH = "Attack1";
        public const string ANIMATION_STAB = "Attack2";

        public void DoFlinch() => SetAnimation(0, ANIMATION_FLINCH, false);
        public void DoTurn() => SetAnimation(0, ANIMATION_TURN, false);
        public void DoSlash() => SetAnimation(0, ANIMATION_SLASH, false);
        public void DoStab() => SetAnimation(0, ANIMATION_STAB, false);
        public void DoDetect() => SetAnimation(0, ANIMATION_DETECT, false);

        public void DoPatrol() => SetAnimation(0, "Move", true).TimeScale = 1f;
        public void DoMove() => SetAnimation(0, "Move", true).TimeScale = 1.4f;
    }
}