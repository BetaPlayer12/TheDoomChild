using Spine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.AI
{
    public static class AIBrainUtility
    {
        public static TrackEntry SetAnimation(this SpineRootAnimation animation, int index, IAIAnimationInfo animationInfo, bool loop)
        {
            var track = animation.SetAnimation(index, animationInfo.animation, loop);
            track.TimeScale = animationInfo.animationTimeScale;
            return track;
        }

        public static TrackEntry AddAnimation(this SpineRootAnimation animation, int index, IAIAnimationInfo animationInfo, bool loop,float delay)
        {
            var track = animation.AddAnimation(index, animationInfo.animation, loop, delay);
            track.TimeScale = animationInfo.animationTimeScale;
            return track;
        }
    }
}