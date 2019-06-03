using Spine.Unity;
using UnityEngine;


namespace DChild.Gameplay.Systems.WorldComponents
{
    [System.Serializable]
    public class SpineObjects : ISpineObjects
    {
        [SerializeField]
        private SkeletonAnimation[] m_skeletonAnimations;
        [SerializeField]
        private float[] m_baseTimeScale;

        public SpineObjects(SkeletonAnimation[] m_skeletonAnimations)
        {
            this.m_skeletonAnimations = m_skeletonAnimations;
            m_baseTimeScale = new float[m_skeletonAnimations.Length];
            for (int i = 0; i < m_baseTimeScale.Length; i++)
            {
                m_baseTimeScale[i] = m_skeletonAnimations[i].timeScale;
            }
        }

        public void AlignTime(float timeScale)
        {
            for (int i = 0; i < m_skeletonAnimations.Length; i++)
            {
                m_skeletonAnimations[i].timeScale = m_baseTimeScale[i] * timeScale;
            }
        }
    }
}