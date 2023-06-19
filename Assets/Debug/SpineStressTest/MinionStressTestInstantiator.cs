using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Spine.Tests
{
    public class MinionStressTestInstantiator : MonoBehaviour
    {
        private List<GameObject> m_instances;

        public SkeletonAnimation InstantiateSpine(SkeletonDataAsset skeletonData, int index)
        {
            var GO = new GameObject($"Instance ({index})");
            var animation = GO.AddComponent<SkeletonAnimation>();
            animation.skeletonDataAsset = skeletonData;
            animation.Initialize(true);

            var animations = skeletonData.GetSkeletonData(true).Animations.ToArray();

            int animationIndex = index;
            if (animations.Length - 1 < index)
            {
                animationIndex = Mathf.CeilToInt(index / animations.Length) - 1;

                if (animationIndex < 0)
                {
                    animationIndex = 0;
                }
                else if (animationIndex >= animations.Length)
                {
                    animationIndex = animations.Length - 1;
                }
            }

            animation.AnimationState.SetAnimation(0, animations[animationIndex], true);

            m_instances.Add(animation.gameObject);

            return animation;
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
            m_instances = new List<GameObject>();
        }
    }
}