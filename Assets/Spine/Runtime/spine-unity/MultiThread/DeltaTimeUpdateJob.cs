using Unity.Jobs;
using Unity.Collections;

namespace DChild
{
    #region Jobs

    public struct DeltaTimeUpdateJob : IJobParallelFor
        {
            private NativeArray<SkeletonTime> m_skeletonTime;
            private float deltaTime;

            public DeltaTimeUpdateJob(NativeArray<SkeletonTime> skeletonTime, float deltaTime)
            {
                m_skeletonTime = skeletonTime;
                this.deltaTime = deltaTime;
            }

            public void Execute(int index)
            {
                var skelTime = m_skeletonTime[index];
                skelTime.deltaTime = deltaTime * skelTime.timeScale;
                skelTime.time += skelTime.deltaTime;
                m_skeletonTime[index] = skelTime;
            }
        }
        #endregion

}