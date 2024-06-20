using Sirenix.OdinInspector;
using System.Collections;
using System.IO;
using UnityEngine;
using Spine.Unity;
using DChild.Gameplay.Characters;
using DChild.Gameplay;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Gameplay.Characters.AI
{

    public abstract class AIBrain<T> : MonoBehaviour where T : IAIInfo
    {
        [System.Serializable]
        public abstract class BaseInfo : IAIInfo
        {
            [System.Serializable]
            public abstract class SkeletonBaseInfo
            {
                //#if UNITY_EDITOR
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

                protected IEnumerable GetSkins()
                {
                    ValueDropdownList<string> list = new ValueDropdownList<string>();
                    var reference = m_skeletonDataAsset.GetAnimationStateData().SkeletonData.Skins.ToArray();
                    for (int i = 0; i < reference.Length; i++)
                    {
                        list.Add(reference[i].Name);
                    }
                    return list;
                }
                //#endif
            }

            [HideReferenceObjectPicker]
            public class BasicAnimationInfo : SkeletonBaseInfo, IAIAnimationInfo
            {
                [SerializeField, ValueDropdown("GetAnimations")]
                private string m_animation;
                [SerializeField, Min(0f)]
                private float m_timeScale = 1;

                public string animation => m_animation;
                public float animationTimeScale => m_timeScale;
            }

            [HideReferenceObjectPicker]
            public class BasicEventInfo : SkeletonBaseInfo
            {
                [SerializeField, ValueDropdown("GetEvents")]
                private string m_eventName;

                public string eventName => m_eventName;
            }

            [System.Serializable, HideReferenceObjectPicker]
            public class SimpleAttackInfo : BasicAnimationInfo
            {
                [SerializeField, MinValue(0)]
                private float m_range;

                public float range => m_range;
            }

            [System.Serializable, HideReferenceObjectPicker]
            public class MovementInfo : BasicAnimationInfo
            {
                [SerializeField, MinValue(0)]
                private float m_speed;

                public float speed => m_speed;
            }

            [System.Serializable, HideReferenceObjectPicker]
            public class SimpleProjectileAttackInfo : BasicAnimationInfo
            {
                [SerializeField, ValueDropdown("GetEvents")]
                private string m_launchOnEvent;
                [SerializeField, MinValue(0)]
                private float m_range;
                [SerializeField]
                private ProjectileInfo m_projectileInfo;

                public string launchOnEvent => m_launchOnEvent;
                public float range => m_range;
                public ProjectileInfo projectileInfo => m_projectileInfo;
            }

            [SerializeField, PreviewField, OnValueChanged("Initialize")]
            protected SkeletonDataAsset m_skeletonDataAsset;

            //#if UNITY_EDITOR
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
            //#endif

            public abstract void Initialize();
        }

        [SerializeField, TabGroup("Reference")]
        protected Character m_character;
        [SerializeField, TabGroup("Reference")]
        protected SpineRootAnimation m_animation;
        [SerializeField, ValueDropdown("GetData"), OnValueChanged("InitializeInfo"), TabGroup("Data")]
        protected AIData m_data;

        [ShowInInspector, HideInEditorMode, TabGroup("Data")]
        protected T m_info;

        public void SetData(AIData data)
        {
            if (data.info.GetType() == typeof(T))
            {
                m_data = data;
            }
        }

        public virtual void ApplyData()
        {
            m_info = (T)m_data.info;
            m_info.Initialize();
            if (m_data.bestiaryData != null)
            {
                m_character.SetID(m_data.bestiaryData.id);
            }
        }


        protected virtual void Awake()
        {
            ApplyData();
        }



#if UNITY_EDITOR
        private IEnumerable GetData()
        {
            var list = new ValueDropdownList<AIData>();
            list.Add("None", null);
            var infoType = typeof(T);
            var filePaths = AssetDatabase.FindAssets("t:AIData");
            for (int i = 0; i < filePaths.Length; i++)
            {

                var asset = AssetDatabase.LoadAssetAtPath<AIData>(AssetDatabase.GUIDToAssetPath(filePaths[i]));
                if (asset != null && asset.info != null && asset.info.GetType() == infoType)
                {
                    list.Add(asset);
                }
            }
            return list;
        }

        private void InitializeInfo()
        {
            m_info = (T)m_data.info;
        }
#endif
    }
}