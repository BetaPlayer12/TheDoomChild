using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Spine.Tests
{
    public class SpineStressTestInstantiator : MonoBehaviour, IStressTestInstantiator<SkeletonDataAsset>
    {
        private List<SkeletonAnimation> m_instances;

        public void Instantiate(SkeletonDataAsset skeletonData, int index)
        {
            var GO = new GameObject($"Instance ({index})");
            var animation = GO.AddComponent<SkeletonAnimation>();
            animation.skeletonDataAsset = skeletonData;
            animation.Initialize(true);

            var aniamtions = skeletonData.GetSkeletonData(true).Animations.ToArray();

            int animationIndex = index;
            if (aniamtions.Length - 1 < index)
            {
                animationIndex = Mathf.CeilToInt(index / aniamtions.Length) - 1;

                if (animationIndex < 0)
                {
                    animationIndex = 0;
                }
                else if (animationIndex >= aniamtions.Length)
                {
                    animationIndex = aniamtions.Length - 1;
                }
            }

            animation.AnimationState.SetAnimation(0, aniamtions[animationIndex], true);

            m_instances.Add(animation);
        }

        [Button]
        public void DestroyAllInstances()
        {
            for (int i = m_instances.Count - 1; i >= 0; i--)
            {
                Destroy(m_instances[i].gameObject);
            }
        }

        private void Awake()
        {
            m_instances = new List<SkeletonAnimation>();
        }
    }
}