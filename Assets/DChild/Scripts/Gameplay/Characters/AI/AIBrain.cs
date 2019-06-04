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
            [SerializeField,PreviewField]
            private SkeletonDataAsset m_skeletonDataAsset;

            protected IEnumerable GetAnimations() => m_skeletonDataAsset.GetAnimationStateData().SkeletonData.Animations;
            protected IEnumerable GetEvents() => m_skeletonDataAsset.GetAnimationStateData().SkeletonData.Events;
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