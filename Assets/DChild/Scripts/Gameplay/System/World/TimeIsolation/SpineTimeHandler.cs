using System.Collections.Generic;
using System.Linq;
using Spine.Unity;

namespace DChild.Gameplay.Systems.WorldComponents
{
    public class SpineTimeHandler
    {
        private float m_timeScale;
        private List<ISpineObjects> m_spineAnimationList;

#if UNITY_EDITOR
        public int registeredObjectCount => m_spineAnimationList.Count;
#endif

        public SpineTimeHandler(float timeScale)
        {
            this.m_timeScale = timeScale;
            m_spineAnimationList = new List<ISpineObjects>();
        }

        public static void AlignTime(SkeletonAnimation[] skeletonAnimations, float timeScale)
        {
            for (int i = 0; i < skeletonAnimations.Length; i++)
            {
                skeletonAnimations[i].timeScale = timeScale;
            }
        }

        public void Register(ISpineObjects spineAnimations)
        {
            spineAnimations.AlignTime(m_timeScale);
            m_spineAnimationList.Add(spineAnimations);
        }

        public void Unregister(ISpineObjects spineAnimations)
        {
            spineAnimations.AlignTime(1f);
            m_spineAnimationList.Remove(spineAnimations);
        }

        public void AlignTime(float timeScale)
        {
            m_timeScale = timeScale;
            for (int i = 0; i < m_spineAnimationList.Count; i++)
            {
                m_spineAnimationList[i].AlignTime(m_timeScale);
            }
        }

        public void ClearNull() => m_spineAnimationList = m_spineAnimationList.Where(item => item != null).ToList();

    }

}