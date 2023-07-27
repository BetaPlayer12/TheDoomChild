using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Spine.Tests
{
    public class MinionStressTestManager : StressTestManager<SkeletonDataAsset>
    {

        [SerializeField]
        private SpineStressTestInstantiator m_instantiator;

        [SerializeField]
        private List<GameObject> m_minionPrefabs;

        protected override IStressTestInstantiator<SkeletonDataAsset> instantiator => m_instantiator;

        protected override IEnumerator TestAllElementsRoutine()
        {
            for (int i = 0; i < m_minionPrefabs.Count; i++)
            {
                var minion = m_minionPrefabs[i];
                var skeletonAnimation = minion.GetComponentInChildren<SkeletonAnimation>();
                yield return StressTestRoutine(skeletonAnimation.SkeletonDataAsset, minion.name);
            }
        }
    }
}