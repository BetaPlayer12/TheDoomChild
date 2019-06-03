
namespace DChild.Gameplay
{
    public class HeartVeinAnimation : SpineAnimation
    {
        public const string ANIMATION_IDLE = "idle anim";
        public const string ANIMATION_BUMP = "bump interaction";
        public const string ANIMATION_BURST = "bursting out anim";

        public const string EVENT_BURST = "Ball Burst";

        public void DoIdle() => SetAnimation(0, ANIMATION_IDLE, true);
        public void DoBump()
        {
            SetAnimation(0, ANIMATION_BUMP, false);
            AddAnimation(0, ANIMATION_IDLE, true, 0f);
        }

        public void DoBurst() => SetAnimation(0, ANIMATION_BURST, false);

        public void StartAsBurstAnimation()
        {
            m_skeletonAnimation.state.ClearTracks();
            DoBurst();
            if (GetCurrentAnimation(0) != null)
            {
                var endTime = m_skeletonAnimation.state.GetCurrent(0).AnimationEnd;
                skeletonAnimation.state.GetCurrent(0).TrackTime = endTime;
            }
        }
    }
}
