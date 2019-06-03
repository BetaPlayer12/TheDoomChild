namespace DChild.Gameplay.Characters.Enemies
{
    public class MeleeBeeDroneAnimation : BeeDroneAnimation
    {
      
        public const string ANIMATION_RAPIDSTING = "Attack1";
        public const string ANIMATION_STINGERDIVE = "Attack2";

        public void DoRapidSting() => SetAnimation(0, ANIMATION_RAPIDSTING, false);
        public void DoStingerDive() => SetAnimation(0, ANIMATION_STINGERDIVE, false);
    }
}