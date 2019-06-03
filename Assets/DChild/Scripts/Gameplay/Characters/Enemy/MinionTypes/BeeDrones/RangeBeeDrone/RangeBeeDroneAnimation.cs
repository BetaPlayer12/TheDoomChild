namespace DChild.Gameplay.Characters.Enemies
{
    public class RangeBeeDroneAnimation : BeeDroneAnimation
    {
        public const string ANIMATION_STINGERDIVE = "Attack1";
        public const string ANIMATION_TOXICSHOT = "Attack2";
        public const string EVENT_PROJECTILE = "Projectile";

        public void DoStingerDive() => SetAnimation(0, ANIMATION_STINGERDIVE, false);
        public void DoToxicShot() => SetAnimation(0, ANIMATION_TOXICSHOT, false);
    }
}

