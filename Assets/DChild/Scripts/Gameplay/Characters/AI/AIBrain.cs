using Sirenix.OdinInspector;
using System.Collections;
using System.IO;
using UnityEngine;
using Spine.Unity;
using DChild.Gameplay.Characters;
using DChild.Gameplay;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Refactor.DChild.Gameplay.Characters.AI
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


            [System.Serializable]
            public abstract class EventBaseInfo : SkeletonBaseInfo
            {
                [SerializeField, ValueDropdown("GetEvents")]
                private string m_animation;

                public string animation => m_animation;

            }

            [System.Serializable, HideReferenceObjectPicker]
            public class SimpleAttackInfo : SkeletonBaseInfo
            {
                [SerializeField, ValueDropdown("GetAnimations")]
                private string m_animation;
                [SerializeField, MinValue(0)]
                private float m_range;

                public string animation => m_animation;
                public float range => m_range;
            }

            [System.Serializable, HideReferenceObjectPicker]
            public class MovementInfo : SkeletonBaseInfo
            {
                [SerializeField, ValueDropdown("GetAnimations")]
                private string m_animation;
                [SerializeField, MinValue(0)]
                private float m_speed;

                public string animation => m_animation;
                public float speed => m_speed;
            }

            public class SimpleProjectileAttackInfo : SkeletonBaseInfo
            {
                [SerializeField, ValueDropdown("GetAnimations")]
                private string m_animation;
                [SerializeField, ValueDropdown("GetEvents")]
                private string m_launchOnEvent;
                [SerializeField]
                private ProjectileInfo m_projectileInfo;

                public string animation => m_animation;
                public string launchOnEvent => m_launchOnEvent;
                public ProjectileInfo projectileInfo => m_projectileInfo;
            }

            [SerializeField, PreviewField, OnValueChanged("Initialize")]
            protected SkeletonDataAsset m_skeletonDataAsset;

#if UNITY_EDITOR
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

            public abstract void Initialize();
        }

        [SerializeField]
        protected Character m_character;
        [SerializeField]
        protected SpineRootAnimation m_animation;
        [SerializeField, ValueDropdown("GetData"), OnValueChanged("InitializeInfo")]
        private AIData m_data;
#if UNITY_EDITOR
        [ShowInInspector, InlineEditor]
        private AIData m_inlineEditor;
#endif

        [ShowInInspector, HideInEditorMode]
        protected T m_info;

        public void SetData(AIData data)
        {
            if (m_data.info.GetType() == m_info.GetType())
            {
                m_data = data;
            }
        }

        public void ApplyData()
        {
            m_info = (T)m_data.info;
            m_info.Initialize();
        }

        protected virtual void Awake()
        {
            ApplyData();
        }
#if UNITY_EDITOR

        [SerializeField, FolderPath, PropertyOrder(-1)]
        private string m_referenceFolder;

        private IEnumerable GetData()
        {
            var list = new ValueDropdownList<AIData>();
            list.Add("None", null);
            var infoType = typeof(T);
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

        private void InitializeInfo()
        {
            m_inlineEditor = m_data;
            m_info = (T)m_data.info;
        }
#endif
    }
}