using DarkTonic.MasterAudio;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using UnityEngine;

namespace DChild
{
    [CreateAssetMenu(fileName = "SpineSoundData", menuName = "DChild/Audio/Spine Sound Data")]
    public class SpineSoundData : ScriptableObject
    {
        [System.Serializable]
        public class Info
        {
            [SerializeField, SoundGroup]
            private string m_soundToPlay;

            public void PlaySound(CallBackSounds callBack)
            {
                if (callBack.IsTransformPlaying(m_soundToPlay) == false)
                {
                    callBack.PlaySound(m_soundToPlay);
                }
            }

            public void StopSound(CallBackSounds callBack)
            {
                callBack.StopSound(m_soundToPlay);
            }

#if UNITY_EDITOR
            protected SkeletonDataAsset m_skeletonDataAsset; 

            public void Initialize(SkeletonDataAsset skeletonData)
            {
                m_skeletonDataAsset = skeletonData;
            }
#endif
        }

        [System.Serializable]
        public class EventInfo : Info
        {
            [SerializeField, ValueDropdown("GetEvents"), PropertyOrder(-1)]
            private string m_eventName;

            public string eventName { get => m_eventName; }

#if  UNITY_EDITOR
            protected IEnumerable GetEvents()
            {
                ValueDropdownList<string> list = new ValueDropdownList<string>();
                var reference = m_skeletonDataAsset.GetAnimationStateData().SkeletonData.Events.ToArray();
                for (int i = 0; i < reference.Length; i++)
                {
                    list.Add(reference[i].Name);
                }
                return list;
            } 
#endif
        }

        [System.Serializable]
        public class AnimationInfo : Info
        {
            [SerializeField, ValueDropdown("GetAnimations"), PropertyOrder(-1)]
            private string m_animationName;

            public string animationName { get => m_animationName; }

#if UNITY_EDITOR
            protected IEnumerable GetAnimations()
            {
                ValueDropdownList<string> list = new ValueDropdownList<string>();
                var reference = m_skeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.ToArray();
                for (int i = 0; i < reference.Length; i++)
                {
                    list.Add(reference[i].Name);
                }
                return list;
            } 
#endif
        }

        [SerializeField]
        private SkeletonDataAsset m_skeletonDataAsset;
        [SerializeField, ListDrawerSettings(NumberOfItemsPerPage = 5), OnValueChanged("InitializeAnimationInfo"), TabGroup("Animation")]
        private AnimationInfo[] m_animationStartInfo;
        [SerializeField, ListDrawerSettings(NumberOfItemsPerPage = 5), OnValueChanged("InitializeEventInfo"), ShowIf("@m_skeletonDataAsset.GetAnimationStateData().SkeletonData.Events.Count > 0"), TabGroup("Event")]
        private EventInfo[] m_eventInfo;

        public int eventCount => m_eventInfo.Length;
        public int animationCount => m_animationStartInfo.Length;

        public AnimationInfo GetAnimationInfo(int index) => m_animationStartInfo[index];

        public EventInfo GetEventInfo(int index) => m_eventInfo[index];

#if UNITY_EDITOR
        private void InitializeEventInfo()
        {
            for (int i = 0; i < m_eventInfo.Length; i++)
            {
                m_eventInfo[i].Initialize(m_skeletonDataAsset);
            }
        }

        private void InitializeAnimationInfo()
        {
            for (int i = 0; i < m_animationStartInfo.Length; i++)
            {
                m_animationStartInfo[i].Initialize(m_skeletonDataAsset);
            }
        } 

        private void OnEnable()
        {
            InitializeEventInfo();
            InitializeAnimationInfo();
        }
#endif
    }

}