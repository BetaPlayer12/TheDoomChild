using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    [CreateAssetMenu(fileName = "DisappearingPlatformData", menuName = "DChild/Gameplay/Environment/Disappearing Platform Data")]
    public class DisappearingPlatformData : ScriptableObject
    {
        [SerializeField]
        private float m_disappearDelay;
        [SerializeField]
        private float m_disappearDuration;

        [SerializeField]
        private SkeletonDataAsset m_skeletonData;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonData"), ShowIf("@m_skeletonData != null")]
        private string m_idleAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonData"), ShowIf("@m_skeletonData != null")]
        private string m_steppedOnAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonData"), ShowIf("@m_skeletonData != null")]
        private string m_aboutToDisappearAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonData"), ShowIf("@m_skeletonData != null")]
        private string m_disappearAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonData"), ShowIf("@m_skeletonData != null")]
        private string m_hiddenAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonData"), ShowIf("@m_skeletonData != null")]
        private string m_reappearAnimation;

        public float disappearDelay => m_disappearDelay;
        public float disappearDuration => m_disappearDuration;
        public string idleAnimation => m_idleAnimation;
        public string steppedOnAnimation => m_steppedOnAnimation;
        public string aboutToDisappearAnimation => m_aboutToDisappearAnimation;
        public string disappearAnimation => m_disappearAnimation;
        public string hiddenAnimation => m_hiddenAnimation;
        public string reappearAnimation => m_reappearAnimation;
    }
}
