using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Spine.Tests
{
    public class SpineStressTestManager : StressTestManager<SkeletonDataAsset>
    {
        [SerializeField]
        private SpineStressTestInstantiator m_instantiator;
        [SerializeField]
        private List<SkeletonDataAsset> m_minionPrefabs;

        protected override IStressTestInstantiator<SkeletonDataAsset> instantiator => m_instantiator;

        protected override IEnumerator TestAllElementsRoutine()
        {
            for (int i = 0; i < m_minionPrefabs.Count; i++)
            {
                var minion = m_minionPrefabs[i];
                yield return StressTestRoutine(minion, minion.name);
            }
        }
    }
}