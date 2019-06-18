using Sirenix.OdinInspector;
using System.Collections;
using System.IO;
using UnityEngine;
using Spine.Unity;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Refactor.DChild.Gameplay.Character.AI
{

    public abstract class AIBrain<T> : MonoBehaviour where T : IAIInfo
    {
        [System.Serializable]
        public abstract class BaseInfo : IAIInfo
        {
            [System.Serializable]
            public abstract class SkeletonBaseInfo
            {
#if UNITY_EDITOR
                protected SkeletonDataAsset m_skeletonDataAsset;

                public void SetData(SkeletonDataAsset skeletonData) => m_skeletonDataAsset = skeletonData;
#endif
            }

            [System.Serializable]
            public abstract class AnimationBaseInfo : SkeletonBaseInfo
            {
                [SerializeField, ValueDropdown("GetAnimations")]
                private string m_animation;

                public string animation => m_animation;

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

            [System.Serializable, HideReferenceObjectPicker]
            public class SimpleAttackInfo : AnimationBaseInfo
            {
                [SerializeField, MinValue(0)]
                private float m_range;

                public float range => m_range;
            }

            [System.Serializable, HideReferenceObjectPicker]
            public class MovementInfo : AnimationBaseInfo
            {
                [SerializeField, MinValue(0)]
                private float m_speed;

                public float speed => m_speed;
            }

            [SerializeField, PreviewField, OnValueChanged("Initialize")]
            protected SkeletonDataAsset m_skeletonDataAsset;

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

            public abstract void Initialize();
        }

        [SerializeField, ValueDropdown("GetData")]
        private AIData m_data;

#if UNITY_EDITOR
        [SerializeField]
#endif
        protected T m_info;

        public void SetData(AIData data)
        {
            if (m_data.info.GetType() == m_info.GetType())
            {
                m_data = data;
            }
        }

        public void ApplyData() => m_info = (T)m_data.info;


#if UNITY_EDITOR
        private void Awake()
        {
            m_info.Initialize();
        }

        [SerializeField, FolderPath, PropertyOrder(-1)]
        private string m_referenceFolder;

        private IEnumerable GetData()
        {
            var list = new ValueDropdownList<AIData>();
            list.Add("None", null);
            var infoType = m_info.GetType();
            var filePaths = Directory.GetFiles(m_referenceFolder);
            for (int i = 0; i < filePaths.Length; i++)
            {
                var asset = AssetDatabase.LoadAssetAtPath<AIData>(filePaths[i]);
                if (asset != null && asset.info.GetType() == infoType)
                {
                    list.Add(asset);
                }
            }
            return list;
        }
#endif
    }
}