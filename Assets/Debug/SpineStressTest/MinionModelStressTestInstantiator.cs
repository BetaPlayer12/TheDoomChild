using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Spine.Tests
{
    public class MinionModelStressTestInstantiator : MonoBehaviour, IStressTestInstantiator<GameObject>
    {
        private List<GameObject> m_instances;

        public void Instantiate(GameObject minionModel, int index)
        {
            var GO = Instantiate(minionModel);
            GO.name = $"Instance ({index})";

            var animations = GO.GetComponent<SkeletonAnimation>().SkeletonDataAsset.GetSkeletonData(true).Animations.ToArray();

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

            GO.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, animations[animationIndex], true);

            m_instances.Add(GO);
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