using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
using DarkTonic.MasterAudio;

namespace DChild.Gameplay.Environment
{
    [CreateAssetMenu(fileName = "DisappearingPlatformData", menuName = "DChild/Gameplay/Environment/Disappearing Platform Data")]
    public class DisappearingPlatformData : ScriptableObject
    {
        [SerializeField]
        private float m_disappearDelay;
        [SerializeField]
        private float m_disappearDuration;

        [SerializeField,BoxGroup("Animation")]
        private SkeletonDataAsset m_skeletonData;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonData"), ShowIf("@m_skeletonData != null"), BoxGroup("Animation")]
        private string m_idleAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonData"), ShowIf("@m_skeletonData != null"), BoxGroup("Animation")]
        private string m_steppedOnAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonData"), ShowIf("@m_skeletonData != null"), BoxGroup("Animation")]
        private string m_aboutToDisappearAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonData"), ShowIf("@m_skeletonData != null"), BoxGroup("Animation")]
        private string m_disappearAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonData"), ShowIf("@m_skeletonData != null"), BoxGroup("Animation")]
        private string m_hiddenAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonData"), ShowIf("@m_skeletonData != null"), BoxGroup("Animation")]
        private string m_reappearAnimation;

        [SerializeField, SoundGroup, BoxGroup("Sound")]
        private string m_aboutToDisappearAudio;
        [SerializeField, SoundGroup, BoxGroup("Sound")]
        private string m_disappearAudio;
        [SerializeField, SoundGroup, BoxGroup("Sound")]
        private string m_reappearAudio;


        public float disappearDelay => m_disappearDelay;
        public float disappearDuration => m_disappearDuration;
        public string idleAnimation => m_idleAnimation;
        public string steppedOnAnimation => m_steppedOnAnimation;
        public string aboutToDisappearAnimation => m_aboutToDisappearAnimation;
        public string disappearAnimation => m_disappearAnimation;
        public string hiddenAnimation => m_hiddenAnimation;
        public string reappearAnimation => m_reappearAnimation;


        public string aboutToDisappearAudio => m_aboutToDisappearAudio;
        public string disappearAudio => m_disappearAudio;
        public string reappearAudio => m_reappearAudio;
    }
}
