using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{
    [CreateAssetMenu(fileName = "TimerEyeAnimationData", menuName = "DChild/Gameplay/Environment/Timer Eye Animation Data")]
    public class TimerEyeAnimationData : ScriptableObject
    {
        [SerializeField]
        private SkeletonDataAsset m_skeletonData;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonData"), ShowIf("@m_skeletonData != null")]
        private string m_openAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonData"), ShowIf("@m_skeletonData != null")]
        private string m_closeAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonData"), ShowIf("@m_skeletonData != null")]
        private string[] m_closeIntervalAnimationList;

        public string openAnimation => m_openAnimation;
        public string closeAnimation => m_closeAnimation;
        public string[] closeIntervalAnimationList => m_closeIntervalAnimationList;
    }
}
