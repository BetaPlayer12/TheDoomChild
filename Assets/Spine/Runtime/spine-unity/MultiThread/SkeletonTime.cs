using Spine.Unity;

namespace DChild
{
    #region Jobs
    public struct SkeletonTime
        {
            public SkeletonTime(SkeletonAnimation animation)
            {
                timeScale = animation.timeScale;
                time = animation.skeleton.Time;
                deltaTime = 0;
            }

            public float timeScale;
            public float time;
            public float deltaTime;
        }
        #endregion

}